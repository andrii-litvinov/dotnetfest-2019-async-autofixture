namespace Contracts.Commands
{
    public class DebitAccount
    {
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}