using Contracts.Events;

namespace Domain
{
    public class Account : AggregateRoot
    {
        public decimal Balance { get; set; }

        public void Debit(decimal value, ulong version)
        {
            CheckVersion(version);
            if (value >= 0) throw new ValidationException("Debit amount must be less than 0.");
            if (Balance < value) throw new ValidationException("Insufficient balance.");
            Balance += value;
            RecordEvent(new AccountDebited(Id, value, Balance));
        }

        public void Credit(decimal value, ulong version)
        {
            CheckVersion(version);
            if (value <= 0) throw new ValidationException("Credit amount must be greater than 0.");
            Balance += value;
            RecordEvent(new AccountCredited(Id, value, Balance));
        }

        public static Account Create(string id)
        {
            var account = new Account {Id = id};
            account.RecordEvent(new AccountCreated(id));
            return account;
        }
    }
}