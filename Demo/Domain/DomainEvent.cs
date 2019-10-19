namespace Domain
{
    public abstract class DomainEvent
    {
        public DomainEvent(string sourceId) => SourceId = sourceId;

        public string SourceId { get; set; }
    }
}