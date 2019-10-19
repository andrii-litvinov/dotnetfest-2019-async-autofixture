namespace Contracts.Events
{
    public abstract class DomainEvent
    {
        public DomainEvent(string sourceId, ulong version)
        {
            SourceId = sourceId;
            Version = version;
        }

        public string SourceId { get; set; }
        public ulong Version { get; set; }
    }
}