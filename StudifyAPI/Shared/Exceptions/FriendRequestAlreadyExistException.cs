namespace StudifyAPI.Shared.Exceptions
{
    public class FriendRequestAlreadyExistException : Exception
    { 
        public FriendRequestAlreadyExistException(string message) : base(message)
        {
        }
    }
}
