using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Ride.Request
{
    public class AcceptRideModel
    {
        public Guid Id { get; set; }

        public string DriverId { get; set; } = string.Empty;
    }
}
