using System.Threading.Tasks;
using Domain;

namespace Service.Persistence
{
    public interface IAccountRepository
    {
        Task Create(Account account);
        Task Update(Account account);
        Task<Account> Find(string commandId);
    }
}