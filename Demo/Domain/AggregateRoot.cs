using System.Collections.Generic;
using Contracts.Events;
using MongoDB.Bson;

namespace Domain
{
    public abstract class AggregateRoot
    {
        // TODO: Consider aggregate version instead of timestamp.

        public string Id { get; set; }
        public BsonTimestamp Timestamp { get; set; } = new BsonTimestamp(0, 0);
        public List<DomainEvent> Events { get; } = new List<DomainEvent>();
        public List<Envelope> Outbox { get; } = new List<Envelope>();
    }
}