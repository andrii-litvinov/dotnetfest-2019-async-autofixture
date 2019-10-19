using System.Collections.Generic;

namespace Domain
{
    public abstract class AggregateRoot
    {
        // TODO: Add Tracing information to the events on save.
        // TODO: Do not read existing events from the DB.

        public List<DomainEvent> Events { get; set; } = new List<DomainEvent>();
        public List<Envelope> Outbox { get; set; } = new List<Envelope>();
    }
}