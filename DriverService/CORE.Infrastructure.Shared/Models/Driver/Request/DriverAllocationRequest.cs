using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Driver.Request
{
    public class DriverAllocationRequest
    {
        public Guid RideId { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
    }
}
