using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.ReviewRide.Request
{
    public class RideReviewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RideId { get; set; } // chuyen xe nao
        public string ReviewerId { get; set; } = string.Empty; //khach hang
        public string TargetUserId { get; set; } = string.Empty; // tai xe
        public double Rating { get; set; } // 1-5 sao
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
