using System.Diagnostics.Contracts;

namespace StudifyAPI.Shared.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string? message) : base(message)
        {
        }
    }
}
