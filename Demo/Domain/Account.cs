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
            if (Version != version) throw new DomainException("Account version does not match.");
            if (value <= 0) throw new DomainException("Credit amount must be greater than 0.");

            Balance += value;
            RecordEvent(v => new AccountCredited(Id, v, value, Balance));
        }

        public void Debit(decimal value, ulong version)
        {
            if (Version != version) throw new DomainException("Account version does not match.");
            if (value >= 0) throw new DomainException("Debit amount must be less than 0.");
            if (Balance + value < 0) throw new DomainException("Insufficient balance.");

            Balance += value;
            RecordEvent(v => new AccountDebited(Id, v, value, Balance));
        }
    }
}