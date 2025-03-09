using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Location.Request
{
    public class DriverLocationUpdateRequest
    {
        public string DriverId { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
