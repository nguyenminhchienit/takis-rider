using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace CORE.Infrastructure.Shared.Models.User.Request
{
    public class UserRequest : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public bool IsDriver { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public bool IsTwoFactorEnabled { get; set; } = false;
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorExpiry { get; set; }
    }
}
