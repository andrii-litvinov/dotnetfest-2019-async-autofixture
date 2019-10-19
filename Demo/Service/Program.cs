using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Service.CommandHandlers;
using Service.Persistence;
using Service.QueryHandlers;
using Service.RequestHandlers;
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
                        container.Register(typeof(IQueryHandler<,>), typeof(Program).Assembly);
                        container.Register<IQueryDispatcher, QueryDispatcher>();
                        container.Register<IAccountRepository, AccountRepository>();
                        container.RegisterSingleton(() => new MongoClient().GetDatabase("dotnetfest"));
                        container.Register<ITraceContextAccessor, TraceContextAccessor>();

                        ConventionRegistry.Register("conventions", new ConventionPack
                        {
                            new CamelCaseElementNameConvention(),
                            new IgnoreExtraElementsConvention(true)
                        }, type => true);

                        BsonClassMap.RegisterClassMap<AggregateRoot>(map =>
                        {
                            map.AutoMap();
                            map
                                .MapMember(root => root.Id)
                                .SetSerializer(new StringSerializer(BsonType.ObjectId));
                            map.UnmapMember(root => root.Events);
                            map.MapMember(root => root.Outbox).SetElementName("_outbox");
                        });

                        BsonClassMap.RegisterClassMap<Envelope>(map =>
                        {
                            map.AutoMap();
                            map.SetDiscriminatorIsRequired(true);
                        });

                        container.Verify();

                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapControllers());
                    }));
    }
}