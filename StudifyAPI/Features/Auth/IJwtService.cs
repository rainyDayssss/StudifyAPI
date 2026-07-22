namespace StudifyAPI.Features.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(string email, int userId);
        string GenerateRefreshToken();
    }
}
