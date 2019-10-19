namespace Contracts.Events
{
    public class Envelope
    {
        public string EventId { get; set; }
        public string CorrelationId { get; set; }
        public string CausationId { get; set; }
        public DomainEvent Event { get; set; }
    }
}