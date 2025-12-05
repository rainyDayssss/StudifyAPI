namespace StudifyAPI.Shared.Exceptions
{
    public class FriendAlreadyExistException : Exception
    {
        public FriendAlreadyExistException(string message) : base(message)
        { }
    }
}
