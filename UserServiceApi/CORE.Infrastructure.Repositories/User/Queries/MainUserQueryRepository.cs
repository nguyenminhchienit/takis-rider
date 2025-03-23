
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User;
using CORE.Infrastructure.Shared.Models.User.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CORE.Infrastructure.Repositories.User.Queries
{
    public class MainUserQueryRepository : IMainUserQuery
    {

        private readonly UserManager<UserModel> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MainUserQueryRepository(UserManager<UserModel> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserRequest?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new UserRequest
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                IsDriver = user.IsDriver
            };
        }

        public async Task<UserRequest?> GetUserCurrent()
        {
            // 🔥 Lấy User ID từ JWT Token
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(email))
                return null;

            // 🔥 Tìm thông tin user trong database
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            return new UserRequest
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                IsDriver = user.IsDriver
            };
        }
    }
}
