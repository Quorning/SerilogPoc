using Microsoft.Extensions.DependencyInjection;

namespace ApplicationContainer
{
    public static class InfrastructureInstaller
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<Infrastructure.Transformation.IJsonSerializer, Infrastructure.Transformation.JsonSerializer>();
            services.AddScoped<Infrastructure.Transformation.IXmlSerializer, Infrastructure.Transformation.XmlSerializer>();

            services.AddTransient<Infrastructure.Trace.ITraceInformation>(sp =>
            {
                var context = sp.GetService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
                return new Infrastructure.Trace.TraceInformation(context);
            });

        }
    }
}
