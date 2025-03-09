using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.Ride.Request;

namespace CORE.Applications.Feature.Ride.Model
{
    public class UpdateRideStatusModel
    {
        public Guid Id { get; set; }

        public RideStatus Status { get; set; }
    }
}
