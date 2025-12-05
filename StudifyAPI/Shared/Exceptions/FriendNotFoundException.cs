using System.Globalization;

namespace StudifyAPI.Shared.Exceptions
{
    public class FriendNotFoundException : Exception
    {
        public FriendNotFoundException(string message) : base(message) { }
    }
}
