using System.Threading.Tasks;
using Contracts.Commands;
using Service.Persistence;

namespace Service.Handlers.Commands
{
    public class DebitAccountHandler : ICommandHandler<DebitAccount>
    {
        private readonly IAccountRepository repository;

        public DebitAccountHandler(IAccountRepository repository) => this.repository = repository;

        public async Task Handle(DebitAccount command)
        {
            var account = await repository.Find(command.AccountId);
            account.Debit(command.Amount, command.Version);
            await repository.Update(account);
        }
    }
}