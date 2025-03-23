using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CORE.Infrastructure.Repositories.Services.EmailService
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioPhoneNumber;

        public SmsService(IConfiguration config)
        {
            _accountSid = config["Twilio:AccountSid"];
            _authToken = config["Twilio:AuthToken"];
            _twilioPhoneNumber = config["Twilio:PhoneNumber"];
        }

        public async Task<bool> SendSmsAsync(string toPhoneNumber, string message)
        {
            try
            {
                // Khởi tạo Twilio Client
                TwilioClient.Init(_accountSid, _authToken);

                // Gửi SMS
                var messageResponse = await MessageResource.CreateAsync(
                    to: new PhoneNumber(toPhoneNumber),
                    from: new PhoneNumber(_twilioPhoneNumber),
                    body: message
                );

                // Kiểm tra trạng thái gửi tin nhắn
                return messageResponse.ErrorCode == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi SMS: {ex.Message}");
                return false;
            }
        }
    }
}
