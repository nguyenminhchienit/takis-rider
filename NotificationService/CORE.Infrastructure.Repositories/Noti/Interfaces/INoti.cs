using CORE.Infrastructure.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Repositories.Noti.Interfaces
{
    public interface INoti
    {
        void SendNotification(string userId, NotificationMessage message);
    }
}
