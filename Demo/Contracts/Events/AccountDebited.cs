namespace Contracts.Events
{
    public class AccountDebited : DomainEvent
    {
        public AccountDebited(string accountId, ulong version, decimal value, decimal amount) : base(accountId, version)
        {
            Value = value;
            Amount = amount;
        }

        public decimal Value { get; set; }
        public decimal Amount { get; set; }
    }
}