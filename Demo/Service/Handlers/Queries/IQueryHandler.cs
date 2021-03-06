using System.Threading.Tasks;
using Contracts.Queries;

namespace Service.Handlers.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}