namespace Contracts.Events
{
    public class Envelope
    {
        public string EventId { get; set; }
        public Trace Trace { get; set; }
        public DomainEvent Event { get; set; }
    }
}