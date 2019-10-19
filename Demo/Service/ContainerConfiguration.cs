using MongoDB.Driver;
using Service.CommandHandlers;
using Service.Persistence;
using Service.QueryHandlers;
using Service.Tracing;
using SimpleInjector;

namespace Service
{
    public static class ContainerConfiguration
    {
        public static void Configure(this Container container)
        {
            container.Register(typeof(ICommandHandler<>), typeof(Program).Assembly);
            container.Register(typeof(IQueryHandler<,>), typeof(Program).Assembly);
            container.Register<IQueryDispatcher, QueryDispatcher>();
            container.Register<IAccountRepository, AccountRepository>();
            container.RegisterSingleton(() => new MongoClient().GetDatabase("dotnetfest"));
            container.Register<ITraceContextAccessor, TraceContextAccessor>();
        }
    }
}