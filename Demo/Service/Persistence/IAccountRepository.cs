using System.Threading.Tasks;
using Domain;

namespace Service.Persistence
{
    public interface IAccountRepository
    {
        Task Create(Account account);
    }
}