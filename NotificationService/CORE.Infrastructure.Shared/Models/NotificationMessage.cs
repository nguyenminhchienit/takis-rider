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
        public NotiObjList Message { get; set; } // Nội dung thông báo
    }

    public class NotiObjList
    {
        public List<DriverInfo> rideDriverQueue { get; set; } = new ();
        public Guid RideId { get; set;}
        public string Message { get; set; } = string.Empty;
    }

    public class DriverInfo
    {
        public string DriverId { get; set; } = string.Empty;
        public double Rating { get; set; }
        public double CancellationRate { get; set; }

        public Guid RideId { get; set; }
        public double Distance { get; set; }
    }
}
