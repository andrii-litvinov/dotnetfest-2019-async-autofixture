using System.Threading.Tasks;
using Contracts.Commands;
using Domain;
using Service.Persistence;

namespace Service.Handlers.Commands
{
    public class CreateAccountHandler : ICommandHandler<CreateAccount>
    {
        private readonly IAccountRepository repository;

        public CreateAccountHandler(IAccountRepository repository) => this.repository = repository;

        public async Task Handle(CreateAccount command)
        {
            var account = Account.Create(command.AccountId);
            await repository.Create(account);
        }
    }
}