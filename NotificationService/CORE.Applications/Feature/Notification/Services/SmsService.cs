using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CORE.Applications.Feature.Notification.Services
{
    public class SmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
            TwilioClient.Init(_configuration["Twilio:AccountSid"], _configuration["Twilio:AuthToken"]);
        }

        public void SendSms(string toNumber, string message)
        {
            MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(_configuration["Twilio:FromNumber"]),
                to: new Twilio.Types.PhoneNumber(toNumber)
            );
        }
    }
}
