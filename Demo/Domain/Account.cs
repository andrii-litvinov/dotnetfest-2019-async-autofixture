using Contracts.Events;

namespace Domain
{
    public class Account : AggregateRoot
    {
        public decimal Balance { get; set; }

        public void Debit(decimal value)
        {
            if (Balance < value) throw new ValidationException("Insufficient balance.");
            Balance -= value;
            Events.Add(new AccountDebited(Id, value, Balance));
        }

        public static Account Create(string id) => new Account
        {
            Id = id,
            Events = {new AccountCreated(id)}
        };
    }
}