using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

namespace Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
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
                        container.Configure();
                    })
                    .Configure(app =>
                    {
                        BsonConfiguration.Configure();
                        app.UseSimpleInjector(container);
                        container.Verify();

                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapControllers());
                    }));
    }
}