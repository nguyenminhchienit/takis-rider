
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

namespace CORE.Infrastructure.Repositories.User.Commands
{
    public class MainUserCommandRepository : IMainUserCommand
    {

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly JwtSettings _jwtConfigs;
        private readonly DbSqlContext dbSqlContext;
        private readonly Authenticate authen;

        public MainUserCommandRepository(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, 
            IOptions<JwtSettings> configuration, DbSqlContext _dbSqlContext, Authenticate _authen)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfigs = configuration.Value;
            dbSqlContext = _dbSqlContext;
            authen = _authen;
        }
        public async Task<AuthResponse> AuthenticateAsync(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new AuthResponse { ErrorMessage = "Không tìm thấy tài khoản" };

            // Xác minh mật khẩu
            bool isVerified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isVerified) return new AuthResponse { ErrorMessage = "Mật khẩu hoặc tài khoản không chính xác" };

            var accessToken = authen.GenerateAccessToken(user);
            var refreshToken = authen.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (new AuthResponse { AccessToken = accessToken, RefreshToken =  refreshToken });
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
