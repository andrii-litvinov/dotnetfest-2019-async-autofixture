using System.Collections.Generic;
using MongoDB.Bson;

namespace Domain
{
    public abstract class AggregateRoot
    {
        // TODO: Add Tracing information to the events on save.
        // TODO: Do not read existing events from the DB.
        // TODO: Consider aggregate version instead of timestamp.

        public BsonTimestamp Timestamp { get; set; } = new BsonTimestamp(0, 0);
        public List<DomainEvent> Events { get; set; } = new List<DomainEvent>();
        public List<Envelope> Outbox { get; set; } = new List<Envelope>();
    }
}