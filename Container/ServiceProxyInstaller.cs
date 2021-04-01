using Microsoft.Extensions.DependencyInjection;

namespace ApplicationContainer
{
    public static class ServiceProxyInstaller
    {
        public static void AddServiceProxy(this IServiceCollection services)
        {
            services.AddScoped<ServiceProxy.Xxxx.IXxxxClient, ServiceProxy.Xxxx.XxxxClient>();
            services.AddScoped<ServiceProxy.Yyyy.IYyyyClient, ServiceProxy.Yyyy.YyyyClient>();
        }
    }
}
