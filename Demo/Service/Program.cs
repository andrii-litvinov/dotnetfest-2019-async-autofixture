using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Service.CommandHandlers;
using Service.Persistence;
using Service.Tracing;
using SimpleInjector;

namespace Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // TODO: Add tracing to the entity/events.

            using var container = new Container();
            await CreateHostBuilder(container, args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(Container container, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                        services.AddSimpleInjector(container, options => options
                            .AddAspNetCore()
                            .AddControllerActivation()
                        );
                    })
                    .Configure(app =>
                    {
                        app.UseSimpleInjector(container);

                        container.Register(typeof(ICommandHandler<>), typeof(Program).Assembly);
                        container.Register<IAccountRepository, AccountRepository>();
                        container.RegisterSingleton(() => new MongoClient().GetDatabase("dotnetfest"));
                        container.Register<ITraceContextAccessor, TraceContextAccessor>();

                        container.Verify();

                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapControllers());
                    }));
    }
}