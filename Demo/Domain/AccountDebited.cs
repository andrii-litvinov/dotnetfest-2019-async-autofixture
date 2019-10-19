namespace Domain
{
    public class AccountDebited : DomainEvent
    {
        public AccountDebited(string sourceId, decimal value, decimal amount) : base(sourceId)
        {
            Value = value;
            Amount = amount;
        }

        public decimal Value { get; set; }
        public decimal Amount { get; set; }
    }
}