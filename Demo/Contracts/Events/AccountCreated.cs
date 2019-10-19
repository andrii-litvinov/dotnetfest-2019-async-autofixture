namespace Contracts.Events
{
    public class AccountCreated : DomainEvent
    {
        public AccountCreated(string id) : base(id)
        {
        }
    }
}