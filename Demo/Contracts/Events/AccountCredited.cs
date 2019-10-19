namespace Contracts.Events
{
    public class AccountCredited : DomainEvent
    {
        public AccountCredited(string accountId, ulong version, decimal value, decimal balance)
            : base(accountId, version)
        {
            Value = value;
            Balance = balance;
        }

        public decimal Value { get; set; }
        public decimal Balance { get; set; }
    }
}