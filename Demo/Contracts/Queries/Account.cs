namespace Contracts.Queries
{
    public class Account
    {
        public string Id { get; set; }
        public ulong Version { get; set; }
        public decimal Balance { get; set; }
    }
}