namespace StudifyAPI.Shared.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException(string message) : base(message)
        {
        }
    }
}
