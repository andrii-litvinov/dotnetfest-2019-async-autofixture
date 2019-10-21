using System.Threading.Tasks;
using Domain;

namespace Service.Persistence
{
    public interface IAccountRepository
    {
        Task<Account> Find(string accountId);
        Task Create(Account account);
        Task Update(Account account);
        Task Delete(string accountId);
    }
}