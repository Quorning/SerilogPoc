using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Serilog.Core;

namespace WebApiWhitSeriLog
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = CreateLogger(configuration);

            try
            {
                Log.Information("Program:Main()");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //TODO: ret solution navn SeriLogPoc
        //TODO: ret klient navn

        private static Logger CreateLogger(IConfigurationRoot configuration)
        {
            // https://dev.to/timothymcgrath/logging-scaffold-for-net-core-serilog-3o91

            // Asynk lokning til fil
            // - https://github.com/serilog/serilog-sinks-async
            // Serilog.Settings.Configuration
            // - https://github.com/serilog/serilog-settings-configuration

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                //.Enrich.FromLogContext()
                //.WriteTo.Console()
                //.WriteTo.EventCollector(
                //    splunkHost: "https://splunk-collector.corp.jyskebank.net",
                //    eventCollectorToken: "3cdb070a-c9ae-47e7-820b-7dd623cd1948",
                //    uriPath: "services/collector/event",
                //    index: "jr_ei_test",
                //    sourceType: "_json",
                //.WriteTo.Console(
                //    formatter: new JsonFormatter(renderMessage: true))
                //.WriteTo.File(
                //    formatter: new JsonFormatter(renderMessage: true),
                //    path: "E:\\Source\\Repos\\Log\\Serililog\\log.json",
                //    rollingInterval: RollingInterval.Minute,
                //    fileSizeLimitBytes: 10000000,
                //    rollOnFileSizeLimit: true,
                //    retainedFileCountLimit: 3)
                //    //retainedFileTimeLimit: 7.00:00:00) //Deletes files older than 7 days
                //.WriteTo.Async(a => a.File(
                //    formatter: new JsonFormatter(renderMessage: true),
                //    path: $"E:\\Source\\Repos\\Log\\Serililog\\logAsync.json",  // Auto-appends the file number to the filename (log-webvm-001.json)
                //    rollingInterval: RollingInterval.Minute,
                //    fileSizeLimitBytes: 10000000, // 10 MB file limit
                //    rollOnFileSizeLimit: true,
                //    retainedFileCountLimit: 3,
                //    buffered: false))
                //Tilføj proppertyes
                .CreateLogger();
        }
    }
}
