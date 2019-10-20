using System.Threading.Tasks;
using Contracts.Commands;
using Service.Persistence;

namespace Service.Handlers.Commands
{
    public class CreditAccountHandler : ICommandHandler<CreditAccount>
    {
        private readonly IAccountRepository repository;

        public CreditAccountHandler(IAccountRepository repository) => this.repository = repository;

        public async Task Handle(CreditAccount command)
        {
            var account = await repository.Find(command.AccountId);
            account.Credit(command.Amount, command.Version);
            await repository.Update(account);
        }
    }
}