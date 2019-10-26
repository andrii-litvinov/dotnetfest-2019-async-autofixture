using System.Threading.Tasks;
using Domain;
using Service.Persistence;
using Tests.Primitives;

namespace Tests.Async
{
    public class AccountInitializer : Initializer
    {
        private readonly Account account;
        private readonly IAccountRepository repository;

        public AccountInitializer(Account account, IAccountRepository repository)
        {
            this.account = account;
            this.repository = repository;
        }
        
        protected override async Task Initialize()
        {
            await repository.Create(account);
            OnDispose = () => repository.Delete(account.Id);
        }
    }
}