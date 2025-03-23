using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.User.Request;

namespace CORE.Infrastructure.Repositories.User.Interfaces
{
    public interface IMainUserQuery
    {
        Task<UserRequest?> GetUserByIdAsync(string id);

        Task<UserRequest?> GetUserCurrent();

    }
}
