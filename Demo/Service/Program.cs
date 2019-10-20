using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Service.Logging;
using Service.Tracing;
using SimpleInjector;
using ILogger = Serilog.ILogger;

namespace Service
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using var container = new Container();
            using var logger = Configuration.CreateLogger();

            try
            {
                await CreateHostBuilder(container, logger, args).Build().RunAsync();
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Service crashed.");
                return 1;
            }

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(Container container, ILogger logger, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                        services.AddSimpleInjector(container, options => options
                            .AddAspNetCore()
                            .AddControllerActivation()
                        );
                        container.ConfigureContainer();
                        container.RegisterInstance(logger);
                        services.AddSingleton<ILoggerFactory>(provider => new SerilogLoggerFactory(logger));
                    })
                    .Configure(app =>
                    {
                        Configuration.ConfigureBson();
                        app.UseSimpleInjector(container, options =>
                        {
                            options.UseMiddleware<TracingMiddleware>(app);
                            options.UseMiddleware<LoggingMiddleware>(app);
                        });
                        container.Verify();

                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapControllers());
                    }));
    }
}