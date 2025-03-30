using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.HubNoti
{
    public class NotificationHub : Hub
    {
        public static Dictionary<string, List<string>> ConnectedUsers = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                lock (ConnectedUsers)
                {
                    if (!ConnectedUsers.ContainsKey(userId))
                        ConnectedUsers[userId] = new List<string>();

                    ConnectedUsers[userId].Add(Context.ConnectionId);
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            lock (ConnectedUsers)
            {
                foreach (var key in ConnectedUsers.Keys)
                {
                    ConnectedUsers[key].Remove(Context.ConnectionId);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
