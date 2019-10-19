namespace Contracts.Events
{
    public class AccountCredited : DomainEvent
    {
        public decimal Value { get; set; }
        public decimal Balance { get; set; }

        public AccountCredited(string accountId, decimal value, decimal balance) : base(accountId)
        {
            Value = value;
            Balance = balance;
        }
    }
}