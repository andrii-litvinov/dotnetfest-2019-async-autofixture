using System;
using Contracts.Events;

namespace Domain
{
    public class Account : AggregateRoot
    {
        public string Id { get; set; }
        public decimal Balance { get; set; }

        public void Debit(decimal value)
        {
            if (Balance < value) throw new ValidationException("Insufficient balance.");
            Balance -= value;
            Events.Add(new AccountDebited(Id, value, Balance));
        }

        public static Account Create(string id)
        {
            return new Account
            {
                Id = id,
                Events = {new AccountCreated(id)}
            };
        }
    }

    public class ValidationException : Exception
    {
        // TODO: Handle in middleware and return BadRequest or Conflict response.

        public ValidationException(string message) : base(message)
        {
        }
    }
}