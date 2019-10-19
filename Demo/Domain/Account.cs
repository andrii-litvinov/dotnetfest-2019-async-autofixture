using Contracts.Events;

namespace Domain
{
    public class Account : AggregateRoot
    {
        public decimal Balance { get; set; }

        public void Debit(decimal value, ulong version)
        {
            CheckVersion(version);
            if (value >= 0)  throw new ValidationException("Debit amount must be less than 0.");
            if (Balance < value) throw new ValidationException("Insufficient balance.");
            Balance += value;
            Events.Add(new AccountDebited(Id, value, Balance));
            Version++;
        }

        public void Credit(decimal value, ulong version)
        {
            CheckVersion(version);
            if (value <= 0)  throw new ValidationException("Credit amount must be greater than 0.");
            Balance += value;
            Version++;
        }

        public static Account Create(string id) => new Account
        {
            Id = id,
            Events = {new AccountCreated(id)}
        };
    }
}