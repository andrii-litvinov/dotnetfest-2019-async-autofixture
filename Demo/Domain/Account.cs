using Contracts.Events;

namespace Domain
{
    public class Account : AggregateRoot
    {
        private Account()
        {
        }

        public decimal Balance { get; set; }

        public static Account Create(string id)
        {
            var account = new Account {Id = id};
            account.RecordEvent(v => new AccountCreated(account.Id, v));
            return account;
        }

        public void Credit(decimal value, ulong version)
        {
            CheckVersion(version);
            if (value <= 0) throw new ValidationException("Credit amount must be greater than 0.");

            Balance += value;
            RecordEvent(v => new AccountCredited(Id, v, value, Balance));
        }

        public void Debit(decimal value, ulong version)
        {
            CheckVersion(version);
            if (value >= 0) throw new ValidationException("Debit amount must be less than 0.");
            if (Balance + value < 0) throw new ValidationException("Insufficient balance.");

            Balance += value;
            RecordEvent(v => new AccountDebited(Id, v, value, Balance));
        }
    }
}