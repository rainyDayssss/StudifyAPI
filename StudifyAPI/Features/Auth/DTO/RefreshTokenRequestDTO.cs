using System.ComponentModel.DataAnnotations;

namespace StudifyAPI.Features.Auth.DTOs
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
