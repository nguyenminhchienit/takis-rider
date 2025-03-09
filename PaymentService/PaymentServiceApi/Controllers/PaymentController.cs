using CORE.Infrastructure.Repositories.Payment.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PaymentServiceApi.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentCommandRepository _paymentService;

        public PaymentController(IPaymentCommandRepository paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("vietqr")]
        public async Task<IActionResult> CreateVietQRPayment(Guid rideId, string userId, decimal amount)
        {
            var qrCodeUrl = await _paymentService.CreateVietQRPaymentAsync(rideId, userId, amount);
            return Ok(new { message = "VietQR Payment Created", qrCodeUrl });
        }

        [HttpPost("vietqr/callback")]
        public async Task<IActionResult> VietQRCallback([FromBody] dynamic data)
        {
            //var transactionId = data.transactionId.ToString();
            //var status = data.status.ToString(); // "Success" hoặc "Failed"

            //var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            //if (transaction == null) return NotFound();

            //transaction.Status = status;
            //await _context.SaveChangesAsync();

            return Ok(new { message = "Payment status updated" });
        }

    }
}
