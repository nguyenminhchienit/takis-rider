
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

using CORE.Infrastructure.Repositories.Config;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.User;
using CORE.Infrastructure.Shared.Models.User.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using CORE.Infrastructure.Repositories.Services.Authen;
using CORE.Infrastructure.Shared.Models.User.Response;
using CORE.Infrastructure.Repositories.Services.EmailService;

namespace CORE.Infrastructure.Repositories.User.Commands
{
    public class MainUserCommandRepository : IMainUserCommand
    {

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly JwtSettings _jwtConfigs;
        private readonly DbSqlContext dbSqlContext;
        private readonly Authenticate authen;
        private readonly EmailService emailService;
        private readonly SmsService smsService;

        public MainUserCommandRepository(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, 
            IOptions<JwtSettings> configuration, DbSqlContext _dbSqlContext, Authenticate _authen, 
            EmailService _emailService, SmsService _smsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfigs = configuration.Value;
            dbSqlContext = _dbSqlContext;
            authen = _authen;
            emailService = _emailService;
            smsService = _smsService;
        }
        public async Task<AuthResponse> AuthenticateAsync(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new AuthResponse { ErrorMessage = "Không tìm thấy tài khoản" };

            // Xác minh mật khẩu
            bool isVerified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isVerified) return new AuthResponse { ErrorMessage = "Mật khẩu hoặc tài khoản không chính xác" };

            // 🔥 Nếu 2FA được bật, tạo mã OTP và gửi qua Email/SMS
            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, user.TwoFactorMethod); // Hoặc "Phone"

                // 🔥 Lưu mã OTP vào database
                /*user.TwoFactorCode = token;
                user.TwoFactorExpiry = DateTime.UtcNow.AddMinutes(5);
                await _userManager.UpdateAsync(user);*/



                // Gửi mã qua Email hoặc SMS
                //await emailService.SendEmailAsync("nguyenminhchien2003@gmail.com", "Mã xác thực 2FA", $"Mã xác thực của bạn là: {token}");

                // 🔥 Gửi mã OTP qua phương thức mà người dùng đã chọn
                if (user.TwoFactorMethod == "Email")
                {
                    await emailService.SendEmailAsync("nguyenminhchien2003@gmail.com", "Mã xác thực 2FA", $"Mã OTP của bạn là: {token}");
                }
                else if (user.TwoFactorMethod == "SMS")
                {
                    await smsService.SendSmsAsync("+84392845906", $"Mã OTP của bạn là: {token}");
                }

                return (new AuthResponse  {Email = user.Email ,ErrorMessage = "Mã xác thực đã được gửi"});
            }

            var accessToken = authen.GenerateAccessToken(user);
            var refreshToken = authen.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (new AuthResponse { Email = user.Email, AccessToken = accessToken, 
                RefreshToken =  refreshToken,  ErrorMessage = "Đăng nhập thành công" });
        }

        public async Task<bool> EnableTwoFactor(Enable2FARequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return false;

            user.TwoFactorEnabled = request.Enable;
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<AuthResponse> VerifyTwoFactor(Verify2FAModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new AuthResponse { ErrorMessage = "Không tìm thấy tài khoản" };

            // Xác thực mã OTP bằng Identity
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Code); // Hoặc "Phone"
            if (!isValid) return new AuthResponse { ErrorMessage = "Mã xác thực không hợp lệ hoặc đã hết hạn" };


            // ✅ Cấp Access Token sau khi xác thực thành công
            var accessToken = authen.GenerateAccessToken(user);
            var refreshToken = authen.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        public async Task<UserRequest?> CreateUserAsync(RegisterModel request)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new UserModel
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Address = request.Address,
                IsDriver = request.IsDriver,
                PasswordHash = hashedPassword,
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return null;

            return new UserRequest
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                IsDriver = user.IsDriver
            };
        }

        public async Task<AuthResponse?> RefreshTokenForDb(RefreshTokenRequest refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken.RefreshToken))
                throw new ArgumentException("Refresh Token là bắt buộc");

            var user1 = await _userManager.Users.FirstAsync();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new ArgumentException("Refresh Token không hợp lệ hoặc đã hết hạn");

            // 🔥 Tạo Access Token mới
            var newAccessToken = authen.GenerateAccessToken(user);
            var newRefreshToken = authen.GenerateRefreshToken();

            // 🔥 Cập nhật Refresh Token mới cho User
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public async Task<bool> RegisterMakeDriverAsync(UserRegisterMakeDriverModel request)
        {
            var userRegister = await dbSqlContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (userRegister == null) return false;
            userRegister.IsDriver = true;
            await dbSqlContext.SaveChangesAsync();

            return true;
        }
    }
}
