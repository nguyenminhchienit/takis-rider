using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Repositories.Routing.Interface
{
    public interface IRoutingService
    {
        Task<string> GetRouteAsync(double startLat, double startLng, double endLat, double endLng);
    }
}
