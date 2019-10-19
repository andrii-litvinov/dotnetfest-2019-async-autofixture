using System;
using System.Threading.Tasks;
using Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using Service.Tracing;

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

        public async Task Create(Account account)
        {
            PopulateOutbox(account);
            await collection.InsertOneAsync(account);
        }

        public async Task Update(Account account)
        {
            PopulateOutbox(account);

            var timestamp = account.Timestamp;
            account.Timestamp = new BsonTimestamp(0, 0);

            var result = await collection.ReplaceOneAsync(
                a => a.Id == account.Id && a.Timestamp == timestamp, account);

            if (result.ModifiedCount == 0)
                throw new Exception($"Cannot update account '{account.Id}' due to optimistic concurrency error.");
        }

        private void PopulateOutbox(AggregateRoot account)
        {
            foreach (var @event in account.Events)
            {
                account.Outbox.Add(new Envelope
                {
                    EventId = Guid.NewGuid().ToString(),
                    Event = @event,
                    CorrelationId = accessor.Trace.CorrelationId,
                    CausationId = accessor.Trace.CausationId
                });
            }
        }
    }
}