using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.Location.Request;

namespace CORE.Infrastructure.Repositories.Location.Interfaces
{
    public interface ILocationQueryRepository
    {
        Task<DriverLocation?> GetDriverLocationAsync(string driverId);
        Task<IEnumerable<string>> FindNearestDriversAsync(double latitude, double longitude, double radiusKm);

        Task<List<DriverInfo>> GetNearbyDrivers(RideAllocationRequest request);
    }

    public class DriverInfo
    {
        public string DriverId { get; set; } = string.Empty;
        public double Rating { get; set; }
        public double CancellationRate { get; set; }

        public Guid RideId { get; set; }
        public double Distance {  get; set; }
    }
}
