using System.Threading.Tasks;
using Contracts.Commands;
using Domain;
using Service.Persistence;

namespace Service.CommandHandlers
{
    public class CreateAccountHandler : ICommandHandler<CreateAccount>
    {
        private readonly IAccountRepository repository;

        public CreateAccountHandler(IAccountRepository repository) => this.repository = repository;

        public async Task Handle(CreateAccount command)
        {
            var account = Account.Create(command.Id);
            await repository.Create(account);
        }
    }
}