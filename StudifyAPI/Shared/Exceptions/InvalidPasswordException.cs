namespace StudifyAPI.Shared.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}
