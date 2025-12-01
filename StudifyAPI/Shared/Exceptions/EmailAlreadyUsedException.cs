namespace StudifyAPI.Common.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException(string message) : base(message)
        {
        }
    }
}
