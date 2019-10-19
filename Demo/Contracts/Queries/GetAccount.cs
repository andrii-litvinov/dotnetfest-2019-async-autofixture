namespace Contracts.Queries
{
    public class GetAccount : IQuery<Account>
    {
        public string Id { get; set; }
    }

    public class Account
    {
        public string Id { get; set; }
        public decimal Balance { get; set; }
    }

    public interface IQuery<TResult>
    {
    }
}