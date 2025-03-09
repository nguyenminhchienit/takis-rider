using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Ride.Request
{
    public class CancleRideModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
