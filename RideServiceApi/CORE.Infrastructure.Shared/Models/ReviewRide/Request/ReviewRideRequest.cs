using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.ReviewRide.Request
{
    public class ReviewRideRequest
    {
        public Guid RideId { get; set; } // chuyen xe nao
        public string ReviewerId { get; set; } = string.Empty;
        public string TargetUserId { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
