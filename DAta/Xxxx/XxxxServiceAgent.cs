using System;
using Domain.ServiceAgents;
using Infrastructure.Transformation;
using Newtonsoft.Json;
using ServiceProxy.Xxxx;
using Infrastructure.Logging;

namespace Data.Xxxx
{
    public class XxxxServiceAgent : IXxxxServiceAgent
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IXxxxClient _xxxxClient;

        private static Serilog.ILogger Log => Serilog.Log.ForContext<XxxxServiceAgent>();

        public XxxxServiceAgent(IJsonSerializer jsonSerializer, IXxxxClient xxxxClient)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _xxxxClient = xxxxClient ?? throw new ArgumentNullException(nameof(xxxxClient));
        }

        public void TestLog()
        {
            try
            {
                CallXxxxApiOperation();
            }
            catch (Exception ex)
            {
                Log.Here().Error("XxxxApi log exception {@Exception}", ex);
            }
        }

        private void CallXxxxApiOperation()
        {
            const string api = "XxxxClient";
            const string operation = "TestOperation";

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var request = new TestOperationRequest()
            {
                RequestProp1 = new Prop1() { Prop11 = "foo", Prop12 = "bar" },
                RequestProp2 = new Prop2() { Prop21 = "foo", Prop22 = "bar" },
                RequestProp3 = "foo",
                RequestProp4 = "bar"
            };

            LogRequest(api, operation, request);

            System.Threading.Thread.Sleep(2150);
            var response = _xxxxClient.TestOperation(request);

            LogRespons(api, operation, stopwatch, response);
        }

        private void LogRequest(string api, string operation, TestOperationRequest request)
        {
            var responsAsJson = _jsonSerializer.ToJson(request, Formatting.Indented);
            using (Serilog.Context.LogContext.PushProperty("Request", responsAsJson, true))
            {
                Log.Here().Information("Request: {@Api} {@Operation}", api, operation);
            }
        }

        private void LogRespons(string api, string operation, System.Diagnostics.Stopwatch stopwatch, TestOperationResponse response)
        {
            var responseAsJson = _jsonSerializer.ToJson(response, Formatting.Indented);
            using (Serilog.Context.LogContext.PushProperty("Respons", responseAsJson, true))
            {
                Log.Here().Information("Respons: {@Api} {@Operation} in {Elapsed:0.0000} ms.", api, operation, stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
