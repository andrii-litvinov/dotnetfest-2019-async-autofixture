using System.Threading.Tasks;
using Contracts.Commands;
using Service.Persistence;

namespace Service.CommandHandlers
{
    public class DebitAccountHandler : ICommandHandler<DebitAccount>
    {
        private readonly IAccountRepository repository;

        public DebitAccountHandler(IAccountRepository repository) => this.repository = repository;

        public async Task Handle(DebitAccount command)
        {
            var account = await repository.Find(command.AccountId);
            account.Debit(command.Amount);
            await repository.Update(account);
        }
    }
}