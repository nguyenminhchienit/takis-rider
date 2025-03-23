using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CORE.Infrastructure.Shared.Models.User
{
    public class UserModel : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public bool IsDriver { get; set; } = false;

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public bool IsTwoFactorEnabled { get; set; } = false;
        public string? TwoFactorCode { get; set; }

        public string TwoFactorMethod { get; set; } = "Email";
        public DateTime? TwoFactorExpiry { get; set; }
    }
}
