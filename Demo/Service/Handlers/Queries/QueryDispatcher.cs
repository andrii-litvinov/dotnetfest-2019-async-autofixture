using System.Threading.Tasks;
using Contracts.Queries;
using SimpleInjector;

namespace Service.Handlers.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly Container container;

        public QueryDispatcher(Container container) => this.container = container;

        public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query)
        {
            var type = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = container.GetInstance(type);
            return await handler.Handle((dynamic) query);
        }
    }
}