using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.User.Request
{
    public class Enable2FARequest
    {
        public string Email { get; set; } = string.Empty;
        public bool Enable { get; set; }
    }
}
