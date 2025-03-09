using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Location.Request
{
    public class DriverData
    {
        public Guid RideId { get; set; }
        public string DriverId { get; set; } = string.Empty;
    }
}
