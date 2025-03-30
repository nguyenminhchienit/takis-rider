using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Noti.Interfaces;
using CORE.Infrastructure.Shared.HubNoti;
using CORE.Infrastructure.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace CORE.Infrastructure.Repositories.Noti.Queries
{
    public class NotiRepo : INoti
    {

        private readonly IHubContext<NotificationHub> _hubContext;

        public NotiRepo(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async void SendNotification(string userId, NotificationMessage message)
        {
            var tasks = new List<Task>();

            foreach(var itemDriver in message.Message.rideDriverQueue)
            {
                if (NotificationHub.ConnectedUsers.TryGetValue(itemDriver.DriverId, out var connections))
                {
                    foreach (var connId in connections)
                    {
                        tasks.Add(_hubContext.Clients.Client(connId).SendAsync("ReceiveNotification", new
                        {
                            Message = "🚗 Bạn có yêu cầu chuyến đi mới!"
                        }));
                    }
                }
            }

            await Task.WhenAll(tasks);
            await Task.Delay(500);
            Console.WriteLine($"Email To: {userId}: {message}");
        }
    }
}
