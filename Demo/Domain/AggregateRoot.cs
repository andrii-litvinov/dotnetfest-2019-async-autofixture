using System;
using System.Collections.Generic;
using Contracts.Events;

namespace Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<DomainEvent> events = new List<DomainEvent>();

        public string Id { get; set; }
        public ulong Version { get; set; }
        public IReadOnlyCollection<DomainEvent> Events => events;
        public List<Envelope> Outbox { get; } = new List<Envelope>();

        protected void CheckVersion(ulong version)
        {
            if (Version != version) throw new ValidationException("Account is already of newer version.");
        }

        protected void RecordEvent(Func<ulong, DomainEvent> func)
        {
            Version++;
            events.Add(func(Version));
        }
    }
}