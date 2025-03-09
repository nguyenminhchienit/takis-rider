
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

namespace CORE.Infrastructure.Repositories.User.Commands
{
    public class MainUserCommandRepository : IMainUserCommand
    {

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly JwtSettings _jwtConfigs;
        private readonly DbSqlContext dbSqlContext;

        public MainUserCommandRepository(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IOptions<JwtSettings> configuration, DbSqlContext _dbSqlContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfigs = configuration.Value;
            dbSqlContext = _dbSqlContext;
        }
        public async Task<string?> AuthenticateAsync(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return null;

            //var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            //if (!result.Succeeded) return null;

            if (request.Password != user.PasswordHash) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfigs.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _jwtConfigs.Issuer,
                Audience = _jwtConfigs.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserRequest?> CreateUserAsync(UserRequest request)
        {
            var user = new UserModel
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Address = request.Address,
                IsDriver = request.IsDriver,
                PasswordHash = request.Password,
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
