using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.User.Request
{
    public class UserRegisterMakeDriverModel
    {
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
       
    }
}
