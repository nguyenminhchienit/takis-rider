using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.Ride.Request;

namespace CORE.Infrastructure.Shared.Models.Ride.Response
{
    public class RideModelResponse
    {
        public Guid Id { get; set; }
        public string PassengerId { get; set; }
        public string? DriverId { get; set; }
        public RideStatus Status { get; set; }
    }
}
