using Microsoft.Extensions.DependencyInjection;

namespace ApplicationContainer
{
    public static class DomaineInstaller
    {
        public static void AddDomaine(this IServiceCollection services)
        {
            services.AddScoped<Domain.ServiceAgents.IXxxxServiceAgent, Data.Xxxx.XxxxServiceAgent>();
            services.AddScoped<Domain.ServiceAgents.IYyyyServiceAgent, Data.Yyyy.YyyyServiceAgent>();

            services.AddScoped<Domain.Serivces.ITestLogningService, Domain.Serivces.TestLogningService>();
        }
    }
}