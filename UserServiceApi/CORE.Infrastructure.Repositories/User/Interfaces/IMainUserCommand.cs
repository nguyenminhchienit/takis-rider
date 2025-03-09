using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.User.Request;

namespace CORE.Infrastructure.Repositories.User.Interfaces
{
    public interface IMainUserCommand
    {
        Task<UserRequest?> CreateUserAsync(UserRequest request);
        Task<string?> AuthenticateAsync(UserLoginRequest request);

        Task<bool> RegisterMakeDriverAsync(UserRegisterMakeDriverModel request);
    }
}
