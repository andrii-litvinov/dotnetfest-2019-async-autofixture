namespace Domain
{
    public class Account : AggregateRoot
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }

        public void Debit(decimal value)
        {
            if (Amount >= value)
            {
                Amount -= value;
                Events.Add(new AccountDebited(Id, value, Amount));
            }
        }
    }
}