using System;
using System.Threading.Tasks;
using Contracts.Events;
using Domain;
using MongoDB.Driver;
using Service.Middleware.Tracing;

namespace Service.Persistence
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ITraceContextAccessor accessor;
        private readonly IMongoCollection<Account> collection;

        public AccountRepository(IMongoDatabase database, ITraceContextAccessor accessor)
        {
            this.accessor = accessor;
            collection = database.GetCollection<Account>("accounts");
        }

        public Task<Account> Find(string accountId) =>
            collection.Find(a => a.Id == accountId).FirstOrDefaultAsync();

        public async Task Create(Account account)
        {
            PopulateOutbox(account);
            await collection.InsertOneAsync(account);
        }

        public async Task Update(Account account)
        {
            PopulateOutbox(account);

            var versionToCheck = account.Version - 1;
            var result = await collection.ReplaceOneAsync(
                a => a.Id == account.Id && a.Version == versionToCheck, account);

            if (result.ModifiedCount == 0)
                throw new Exception($"Cannot update account '{account.Id}' due to optimistic concurrency error.");
        }

        public async Task Delete(string accountId) =>
            await collection.DeleteOneAsync(account => account.Id == accountId);

        private void PopulateOutbox(AggregateRoot account)
        {
            foreach (var @event in account.Events)
                account.Outbox.Add(new Envelope
                {
                    EventId = Guid.NewGuid().ToString(),
                    Event = @event,
                    Trace = accessor.Trace
                });
        }
    }
}