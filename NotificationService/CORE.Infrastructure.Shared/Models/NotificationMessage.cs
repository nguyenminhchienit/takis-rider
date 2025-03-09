using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models
{
    public class NotificationMessage
    {
        public string UserId { get; set; } = string.Empty;   // ID của người nhận thông báo
        public string Message { get; set; } = string.Empty; // Nội dung thông báo
    }
}
