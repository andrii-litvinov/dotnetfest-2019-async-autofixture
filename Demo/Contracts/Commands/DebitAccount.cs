namespace Contracts.Commands
{
    public class DebitAccount
    {
        public string AccountId { get; set; }
        public ulong Version { get; set; }
        public decimal Amount { get; set; }
    }
}