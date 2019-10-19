using System;

namespace Domain
{
    public class ValidationException : Exception
    {
        // TODO: Handle in middleware and return BadRequest or Conflict response.

        public ValidationException(string message) : base(message)
        {
        }
    }
}