using System.Collections.Generic;
using Contracts.Events;

namespace Domain
{
    public abstract class AggregateRoot
    {
        // TODO: Consider aggregate version instead of timestamp.

        public string Id { get; set; }
        public ulong Version { get; set; }
        public List<DomainEvent> Events { get; } = new List<DomainEvent>();
        public List<Envelope> Outbox { get; } = new List<Envelope>();
    }
}