using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ApplicationContainer;
using Infrastructure.Settings;
using WebApiWithSeriLog.Filter;
using WebApiWithSeriLog.Middleware;

namespace WebApiWhitSeriLog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GlobalSettings>(Configuration.GetSection("GlobalSettings"));

            services.AddControllers(); 
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AddCommonHeaderParameOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiWithSeriLogger", Version = "v1" });
            });

            Serilog.Debugging.SelfLog.Enable(System.Console.Out);
            System.Threading.Thread.Sleep(3000);

            services.AddHttpContextAccessor();
            services.AddInfrastructure();
            services.AddDomaine();
            services.AddServiceProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiWhitSeriLogger v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseRequestResponseLogging();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
