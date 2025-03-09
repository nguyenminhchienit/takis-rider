using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Ride.Request
{

    public enum RideStatus
    {
        Pending, // Chưa có tài xế nhận
        Accepted, // Đã có tài xế nhận
        Arrived, // Da den diem don
        InProgress, // Đang di chuyển
        Completed, // Hoàn thành
        Canceled, // Hủy chuyến
    }
    public class RideModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string PassengerId { get; set; } = string.Empty;

        public string? DriverId { get; set; }

        [Required]
        public double PickupLatitude { get; set; }

        [Required]
        public double PickupLongitude { get; set; }

        [Required]
        public double DropoffLatitude { get; set; }

        [Required]
        public double DropoffLongitude { get; set; }

        [Required]
        public RideStatus Status { get; set; } = RideStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
