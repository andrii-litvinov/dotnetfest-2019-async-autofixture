using System;
using System.Threading.Tasks;
using Domain;
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