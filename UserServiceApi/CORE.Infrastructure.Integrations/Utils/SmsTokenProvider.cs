using Microsoft.AspNetCore.Identity;


namespace CORE.Infrastructure.Integrations.Utils
{
    public class SmsTokenProvider<TUser> : PhoneNumberTokenProvider<TUser> where TUser : class
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(true);
        }
    }
}
