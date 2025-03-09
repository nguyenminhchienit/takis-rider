using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Driver.Request
{
    public class DriverModelRequest
    {
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty; // Liên kết với UserService
        public string LicensePlate { get; set; } = string.Empty;
        public DriverStatus Status { get; set; }
    }
}
