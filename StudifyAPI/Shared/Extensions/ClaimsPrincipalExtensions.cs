using System.Security.Claims;

namespace StudifyAPI.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (!int.TryParse(user.FindFirst("userId")?.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user token.");
            }
            return userId;
        }
    }
}
