using MongoDB.Driver;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Service.Handlers.Commands;
using Service.Handlers.Commands.Decorators;
using Service.Handlers.Queries;
using Service.Handlers.Queries.Decorators;
using Service.Middleware.Tracing;
using Service.Persistence;
using SimpleInjector;

namespace Service
{
    public static class Configuration
    {
        public static void ConfigureContainer(this Container container)
        {
            container.RegisterSingleton(() => new MongoClient().GetDatabase("dotnetfest"));

            container.Register(typeof(ICommandHandler<>), typeof(Configuration).Assembly);
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandLoggingDecorator<>));

            container.Register(typeof(IQueryHandler<,>), typeof(Configuration).Assembly);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(QueryLoggingDecorator<,>));

            container.Register<IQueryDispatcher, QueryDispatcher>();
            container.Register<IAccountRepository, AccountRepository>();
            container.Register<ITraceContextAccessor, TraceContextAccessor>();
        }

        public static Logger CreateLogger() =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.Async(c => c.Console(
//                    new RenderedCompactJsonFormatter(),
                    LogEventLevel.Debug
                ))
                .Enrich.WithDemystifiedStackTraces()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", typeof(Configuration).Assembly.GetName().Name)
                .CreateLogger();
    }
}