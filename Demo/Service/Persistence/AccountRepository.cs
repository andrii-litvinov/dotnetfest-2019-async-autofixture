using System;
using System.Threading.Tasks;

namespace Account.Persistence
{
    public class AccountRepository : IAccountRepository
    {
        public Task Create(Domain.Account account) => throw new NotImplementedException();
    }

    public interface IAccountRepository
    {
        Task Create(Domain.Account account);
    }
}