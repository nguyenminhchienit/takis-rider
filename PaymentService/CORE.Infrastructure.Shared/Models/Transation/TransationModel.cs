using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Models.Transation
{
    public class TransationModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RideId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }  // "VietQR"

        [Required]
        public string Status { get; set; } = "Pending";  // "Pending", "Success", "Failed"

        public string QrCodeUrl { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
