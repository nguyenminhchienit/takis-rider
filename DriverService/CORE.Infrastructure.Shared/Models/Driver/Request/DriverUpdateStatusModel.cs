using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Driver.Request
{
    public class DriverUpdateStatusModel
    {
        public string driverId { get; set; } = string.Empty;

        public DriverStatus Status { get; set; } = DriverStatus.Offline;
    }
}
