using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Shared.Models.Location.Request;
using Newtonsoft.Json;
using StackExchange.Redis;
using static CORE.Infrastructure.Repositories.Location.Commands.LocationCommandRepository;

namespace CORE.Infrastructure.Repositories.Location.Queries
{
    public class LocationQueryRepository : ILocationQueryRepository
    {

        private readonly IConnectionMultiplexer _redis;

        public LocationQueryRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task<IEnumerable<string>> FindNearestDriversAsync(double latitude, double longitude, double radiusKm)
        {
            var db = _redis.GetDatabase();
            var results = await db.GeoRadiusAsync("drivers:locations", longitude, latitude, radiusKm, GeoUnit.Kilometers);
            return (IEnumerable<string>)results.Select(r => r.Member);
        }

        public async Task<DriverLocation?> GetDriverLocationAsync(string driverId)
        {
            var db = _redis.GetDatabase();
            string key = $"driver:{driverId}:location";
            string? value = await db.StringGetAsync(key);
            return value != null ? System.Text.Json.JsonSerializer.Deserialize<DriverLocation>(value) : null;
        }

        public async Task<List<DriverInfo>> GetNearbyDrivers(RideAllocationRequest request)
        {
            string redisKey = "drivers:locations";
            var geoRadiusResult = await _redis.GetDatabase().GeoRadiusAsync(redisKey, request.PickupLongitude, request.PickupLatitude, 5, GeoUnit.Kilometers);

            
            var nearbyDrivers = geoRadiusResult
                .Select(driver =>
                {
                    var parts = driver.Member.ToString().Split(':'); // Tách driverId và rideId
                    return new DriverInfo
                    {
                        DriverId =(parts[0]).ToString(),
                        RideId = Guid.Parse(parts[1]),
                        Distance = (double)(driver.Distance ?? 0)
                    };
                })
                .OrderBy(d => d.Distance)
                .ToList();

            var mockNearbyDrivers = new List<DriverInfo>
            {
                new DriverInfo { DriverId = "driver_001", RideId = Guid.NewGuid(), Distance = 1.2 },
                new DriverInfo { DriverId = "driver_002", RideId = Guid.NewGuid(), Distance = 2.5 },
                new DriverInfo { DriverId = "driver_003", RideId = Guid.NewGuid(), Distance = 0.8 },
                new DriverInfo { DriverId = "driver_004", RideId = Guid.NewGuid(), Distance = 3.1 },
                new DriverInfo { DriverId = "driver_005", RideId = Guid.NewGuid(), Distance = 1.7 }
            };


            return mockNearbyDrivers;
        }

    }
}
