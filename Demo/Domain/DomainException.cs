using System;

namespace Domain
{
    public class DomainException : Exception
    {
        // TODO: Handle in middleware and return BadRequest or Conflict response.

        public DomainException(string message) : base(message)
        {
        }
    }
}