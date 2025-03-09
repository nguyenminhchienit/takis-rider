using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Noti.Interfaces;

namespace CORE.Infrastructure.Repositories.Noti.Queries
{
    public class NotiRepo : INoti
    {
        public async void SendNotification(string userId, string message)
        {
            // Giả lập gửi email/SMS
            await Task.Delay(500);
            Console.WriteLine($"📩 Gửi thông báo đến {userId}: {message}");
        }
    }
}
