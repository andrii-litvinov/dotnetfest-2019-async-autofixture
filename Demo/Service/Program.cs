using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Service.Middleware.ExceptionHandling;
using Service.Middleware.Logging;
using Service.Middleware.Tracing;
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
                await CreateHostBuilder(container, logger, args, true).Build().RunAsync();
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Service crashed.");
                return 1;
            }

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(
            Container container, ILogger logger, string[] args, bool verify = false) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => { ConfigureWebHost(container, logger, verify, builder); });

        public static IWebHostBuilder ConfigureWebHost(
            Container container, ILogger logger, bool verify = false, IWebHostBuilder builder = null) =>
            (builder ?? new WebHostBuilder())
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
                BsonConfiguration.Configure();
                app.UseSimpleInjector(container, options =>
                {
                    options.UseMiddleware<TracingMiddleware>(app);
                    options.UseMiddleware<LoggingContextMiddleware>(app);
                    options.UseMiddleware<DomainExceptionHandlingMiddleware>(app);
                });

                if (verify) container.Verify();

                app.UseRouting();
                app.UseEndpoints(endpoints => endpoints.MapControllers());
            });
    }
}