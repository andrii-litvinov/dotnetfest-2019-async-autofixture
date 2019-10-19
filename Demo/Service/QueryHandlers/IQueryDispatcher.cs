using System.Threading.Tasks;
using Contracts.Queries;

namespace Service.QueryHandlers
{
    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TResult>(IQuery<TResult> query);
    }
}