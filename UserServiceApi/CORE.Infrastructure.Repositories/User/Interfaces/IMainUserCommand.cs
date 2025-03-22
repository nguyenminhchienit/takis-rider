using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.User;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Models.User.Response;

namespace CORE.Infrastructure.Repositories.User.Interfaces
{
    public interface IMainUserCommand
    {
        Task<UserRequest?> CreateUserAsync(RegisterModel request);
        Task<AuthResponse?> RefreshTokenForDb(RefreshTokenRequest refreshToken);
        Task<AuthResponse> AuthenticateAsync(UserLoginRequest request);

        Task<bool> RegisterMakeDriverAsync(UserRegisterMakeDriverModel request);
    }
}
