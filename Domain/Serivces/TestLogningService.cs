using System;
using System.Reflection;
using Domain.ServiceAgents;
using Infrastructure.Logging;

namespace Domain.Serivces
{
    public interface ITestLogningService
    {
        void TestLog();
    }

    public class TestLogningService : ITestLogningService
    {
        private static Serilog.ILogger Log => Serilog.Log.ForContext<TestLogningService>();

        private readonly IXxxxServiceAgent _xxxxServiceAgent;
        private readonly IYyyyServiceAgent _yyyyServiceAgent;

        public TestLogningService(IXxxxServiceAgent xxxxServiceAgent, IYyyyServiceAgent yyyyServiceAgent)
        {
            _xxxxServiceAgent = xxxxServiceAgent ?? throw new ArgumentNullException(nameof(xxxxServiceAgent));
            _yyyyServiceAgent = yyyyServiceAgent ?? throw new ArgumentNullException(nameof(yyyyServiceAgent));
        }

        public void TestLog()
        {
            Log.Here().Information("TestLogningInDomain LogDebug text.");

            try
            {
                _xxxxServiceAgent.TestLog();
                _yyyyServiceAgent.TestLog();

                throw new NotImplementedException("NotImplementedException");
            }
            catch (Exception ex)
            {
                Log.Here().Error("TestLogningInDomain log exception {@Exception}", ex);
            }
        }
    }
}
