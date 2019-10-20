using System.Threading.Tasks;
using Contracts.Queries;

namespace Service.Handlers.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TResult>(IQuery<TResult> query);
    }
}