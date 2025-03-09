using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Ride.Response
{
    public class UserInfoResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsDriver { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}
