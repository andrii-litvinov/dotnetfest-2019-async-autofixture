using System.Threading.Tasks;
using Contracts.Queries;
using MongoDB.Driver;

namespace Service.QueryHandlers
{
    public class GetAccountHandler : IQueryHandler<GetAccount, Account>
    {
        private readonly IMongoCollection<Domain.Account> collection;

        public GetAccountHandler(IMongoDatabase database) =>
            collection = database.GetCollection<Domain.Account>("accounts");

        public Task<Account> Handle(GetAccount query) => collection
            .Find(account => account.Id == query.Id)
            .Project(account => new Account
            {
                Id = account.Id,
                Balance = account.Balance
            })
            .FirstOrDefaultAsync();
    }
}