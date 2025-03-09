
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User;
using CORE.Infrastructure.Shared.Models.User.Request;
using Microsoft.AspNetCore.Identity;

namespace CORE.Infrastructure.Repositories.User.Queries
{
    public class MainUserQueryRepository : IMainUserQuery
    {

        private readonly UserManager<UserModel> _userManager;

        public MainUserQueryRepository(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
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
    }
}
