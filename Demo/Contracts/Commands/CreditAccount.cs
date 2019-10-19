namespace Contracts.Commands
{
    public class CreditAccount
    {
        public string AccountId { get; set; }
        public ulong Version { get; set; }
        public decimal Amount { get; set; }
    }
}