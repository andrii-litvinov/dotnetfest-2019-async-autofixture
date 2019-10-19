namespace Contracts.Events
{
    public class AccountCreated : DomainEvent
    {
        public AccountCreated(string accountId, ulong version) : base(accountId, version)
        {
        }
    }
}