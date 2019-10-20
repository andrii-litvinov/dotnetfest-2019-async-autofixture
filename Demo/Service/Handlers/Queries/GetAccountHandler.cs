using System.Threading.Tasks;
using Contracts.Queries;
using MongoDB.Driver;

namespace Service.Handlers.Queries
{
    public class GetAccountHandler : IQueryHandler<GetAccount, Account>
    {
        private readonly IMongoCollection<Domain.Account> collection;

        public GetAccountHandler(IMongoDatabase database) =>
            collection = database.GetCollection<Domain.Account>("accounts");

        public Task<Account> Handle(GetAccount query) => collection
            .Find(account => account.Id == query.AccountId)
            .Project(account => new Account
            {
                Id = account.Id,
                Version = account.Version,
                Balance = account.Balance
            })
            .FirstOrDefaultAsync();
    }
}