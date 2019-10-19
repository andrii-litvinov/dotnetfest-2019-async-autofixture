namespace Contracts.Queries
{
    public class GetAccount : IQuery<Account>
    {
        public string AccountId { get; set; }
    }
}