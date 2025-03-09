using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Payment.Interfaces;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.Transation;
using Microsoft.Extensions.Configuration;

namespace CORE.Infrastructure.Repositories.Payment.Commands
{
    public class PaymentCommnadRepository : IPaymentCommandRepository
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly DbSqlContext _context;

        public PaymentCommnadRepository(IConfiguration configuration, HttpClient httpClient, DbSqlContext context)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _context = context;
        }
        public async Task<string> CreateVietQRPaymentAsync(Guid rideId, string userId, decimal amount)
        {
            var requestBody = new
            {
                bankId = _configuration["VietQR:BankId"],
                accountNumber = _configuration["VietQR:AccountNumber"],
                accountName = _configuration["VietQR:AccountName"],
                amount = amount,
                memo = $"Thanh toán cho chuyến đi {rideId}",
                template = "compact"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["VietQR:BaseUrl"]}", jsonContent);
            var responseData = await response.Content.ReadAsStringAsync();

            var qrCodeUrl = $"https://chart.googleapis.com/chart?chs=250x250&cht=qr&chl={responseData}";

            var transaction = new TransationModel
            {
                Id = Guid.NewGuid(),
                RideId = rideId,
                UserId = userId,
                Amount = amount,
                PaymentMethod = "VietQR",
                TransactionId = responseData,
                QrCodeUrl = qrCodeUrl
            };

            _context.Transations.Add(transaction);
            await _context.SaveChangesAsync();

            return qrCodeUrl;
        }
    }
}
