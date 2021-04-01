using Microsoft.AspNetCore.Http;
using System;

namespace Infrastructure.Trace
{
    public interface ITraceInformation
    {
    }

    public class TraceInformation : ITraceInformation
    {
        public Guid TraceId { get; }
        public Guid InitielTraceId { get; }
        public string CallSystem { get; }
        public string UserId { get; }

        //private readonly IHttpContextAccessor _httpContext;

        public TraceInformation(IHttpContextAccessor httpContext)
        {
            httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));

            var context = httpContext.HttpContext;

            if (context == null) return;

            var userId = TraceHeaderReader.GetHeaderValue(TraceHeaderNames.UserId, context.Request.Headers);
            var callSystem = TraceHeaderReader.GetHeaderValue(TraceHeaderNames.CallSystem, context.Request.Headers);
            var traceId = TraceHeaderReader.GetHeaderValuesGuid(context.Request.Headers, TraceHeaderNames.TraceId);
            var initielTraceId = TraceHeaderReader.GetHeaderValuesGuid(context.Request.Headers, TraceHeaderNames.InitielTraceId);

            if (traceId == Guid.Empty)
            {
                traceId = Guid.NewGuid();
            }

            if (initielTraceId == Guid.Empty)
            {
                initielTraceId = traceId;
            }

            TraceId = traceId;
            InitielTraceId = initielTraceId;
            CallSystem = callSystem;
            UserId = userId;
        }
    }
}
