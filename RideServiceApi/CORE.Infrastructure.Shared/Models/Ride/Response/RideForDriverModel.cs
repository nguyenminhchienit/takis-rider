using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.Ride.Request;

namespace CORE.Infrastructure.Shared.Models.Ride.Response
{
    public class RideForDriverModel
    {
        public Guid Id { get; set; }
        public UserInfoResponse? PassengerInfo { get; set; }
        public RideStatus Status { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
    }
}
