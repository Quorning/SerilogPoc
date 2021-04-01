using System;
using System.Reflection;
using Domain.ServiceAgents;
using Infrastructure.Logging;
using Infrastructure.Transformation;
using ServiceProxy.Yyyy;

namespace Data.Yyyy
{
    public class YyyyServiceAgent : IYyyyServiceAgent
    {
        private static Serilog.ILogger Log => Serilog.Log.ForContext<YyyyServiceAgent>();

        private readonly IXmlSerializer _xmlSerializer;
        private readonly IYyyyClient _yyyyClient;

        public YyyyServiceAgent(IXmlSerializer xmlSerializer, IYyyyClient yyyyClient)
        {
            _xmlSerializer = xmlSerializer ?? throw new ArgumentNullException(nameof(xmlSerializer));
            _yyyyClient = yyyyClient ?? throw new ArgumentNullException(nameof(yyyyClient));
        }

        public void TestLog()
        {
            try
            {
                CallYyyyApiOperation();
            }
            catch (Exception ex)
            {
                Log.Here().Error("YyyyService log exception {@Exception}", ex);
            }
        }

        private void CallYyyyApiOperation()
        {
            const string service = "YyyyClient";
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

            LogRequest(service, operation, request);

            System.Threading.Thread.Sleep(1750);
            var response = _yyyyClient.TestOperation(request);

            LogRespons(service, operation, stopwatch, response);
        }

        private void LogRequest(string service, string operation, TestOperationRequest request)
        {
            var requestAsXml = _xmlSerializer.ToXml(request);
            using (Serilog.Context.LogContext.PushProperty("Request", requestAsXml, true))
            {
                Log.Here().Information("Request: {@service} {@Operation}", service, operation);
            }
        }

        private void LogRespons(string service, string operation, System.Diagnostics.Stopwatch stopwatch, TestOperationResponse response)
        {
            var responsAsXml = _xmlSerializer.ToXml(response);
            using (Serilog.Context.LogContext.PushProperty("Respons", responsAsXml, true))
            {
                Log.Here().Information("Respons: {@Api} {@Operation} in {Elapsed:0.0000} ms.", service, operation, stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
