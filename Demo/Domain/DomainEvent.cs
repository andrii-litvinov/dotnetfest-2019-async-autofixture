namespace Domain
{
    public abstract class DomainEvent
    {
        // TODO: Include event id into the event.

        public DomainEvent(string sourceId) => SourceId = sourceId;

        public string SourceId { get; set; }
    }
}