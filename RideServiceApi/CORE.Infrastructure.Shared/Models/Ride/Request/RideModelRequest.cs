using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Ride.Request
{
    public class RideModelRequest
    {
        public string PassengerId { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
    }
}
