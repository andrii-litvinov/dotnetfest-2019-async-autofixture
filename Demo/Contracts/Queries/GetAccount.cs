namespace Contracts.Queries
{
    public class GetAccount : IQuery<Account>
    {
        public string Id { get; set; }
    }
}