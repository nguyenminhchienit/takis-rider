using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.Location.Request;

namespace CORE.Infrastructure.Repositories.Location.Interfaces
{
    public interface ILocationCommandRepository
    {
        public Task<List<string>> GetOnlineDriversAsync();
        public Task<bool> UpdateDriverLocationAsync(string driverId, double latitude, double longitude);

        public Task SetDriverOnlineAsync(string driverId);

        public Task SetDriverOfflineAsync(string driverId);
    }
}
