using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Infrastructure.Trace;
using Infrastructure.Logging;

namespace WebApiWithSeriLog.Middleware
{
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }

    public class RequestResponseLoggingMiddleware
    {
        private static Serilog.ILogger Log => Serilog.Log.ForContext<RequestResponseLoggingMiddleware>();

        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private ITraceInformation _traceInformation;
        private System.Diagnostics.Stopwatch _stopwatch;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ITraceInformation traceInformation)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _traceInformation = traceInformation;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context, ITraceInformation traceInformation)
        {
            _traceInformation = traceInformation ?? throw new ArgumentNullException(nameof(traceInformation));

            _stopwatch = new System.Diagnostics.Stopwatch();
            _stopwatch.Start();

            //TODO: Log TraceIdentifier ??
            using (Serilog.Context.LogContext.PushProperty("TraceInformation", _traceInformation, true))
            {
                await LogRequest(context);
                await LogResponse(context);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            //TODO: er det sådan jeg vil logge Headers eller skal det være noget JSON
            using (Serilog.Context.LogContext.PushProperty("HttpRequestBody", ReadStreamInChunks(requestStream), true))
            using (Serilog.Context.LogContext.PushProperty("HttpRequestHeader", context.Request.Headers.Select(x => x.Key + " : " + x.Value), true))
            {
                Log.Here().Information("Request: {@Method} {@DisplayUrl}",
                    context.Request.Method,
                    context.Request.GetDisplayUrl());
            }

            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            //TODO: er det sådan jeg vil logge Headers eller skal det være noget JSON
            using (Serilog.Context.LogContext.PushProperty("HttpResponsBody", body, true))
            using (Serilog.Context.LogContext.PushProperty("HttpResponsHeader", context.Response.Headers.Select(x => x.Key + " : " + x.Value), true))
            {
                _stopwatch.Stop();

                if (context.Response.StatusCode == 200)
                {
                    Log.Here().Information("Respons: {@Method} {@DisplayUrl}, status code {StatusCode} in {Elapsed:0.0000} ms",
                    context.Request.Method,
                    context.Request.GetDisplayUrl(),
                    context.Response.StatusCode,
                    _stopwatch.Elapsed.TotalMilliseconds);
                }
                else
                {
                    //TODO: er det en god ide ?? hvis ja finde en smart løsning
                    Log.Here().Warning("Respons: {@Method} {@DisplayUrl}, status code {StatusCode} in {Elapsed:0.0000} ms",
                        context.Request.Method,
                        context.Request.GetDisplayUrl(),
                        context.Response.StatusCode,
                        _stopwatch.Elapsed.TotalMilliseconds);
                }
            }
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
