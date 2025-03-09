using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Driver.Request
{

    public enum DriverStatus
    {
        Offline,
        Online,
        Busy
    }
    public class DriverModel
    {

        [Key]
        [Required]
        public string Id { get; set; } = string.Empty;// Liên kết với UserService UserId

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        public string LicensePlate { get; set; } = string.Empty;

        public DriverStatus Status { get; set; } = DriverStatus.Offline;
    }
}
