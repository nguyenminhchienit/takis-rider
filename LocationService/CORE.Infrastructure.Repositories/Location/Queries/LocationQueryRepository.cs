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

            // Kết nối Redis
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase(1); // Đảm bảo đúng database
            await db.KeyDeleteAsync("drivers:locations");
            // TP.HCM
            await db.GeoAddAsync(redisKey, 106.7000, 10.7769, $"driver_hcm_1:{request.RideId}");
            await db.GeoAddAsync(redisKey, 106.7030, 10.7800, $"driver_hcm_2:{request.RideId}");
            await db.GeoAddAsync(redisKey, 106.6980, 10.7735, $"driver_hcm_3:{request.RideId}");
            await db.GeoAddAsync(redisKey, 106.7100, 10.7850, $"driver_hcm_4:{request.RideId}");
            await db.GeoAddAsync(redisKey, 106.7200, 10.7920, $"driver_hcm_5:{request.RideId}");

            // Hà Nội
            await db.GeoAddAsync(redisKey, 105.8544, 21.0285, $"driver_hn_1:{request.RideId}");
            await db.GeoAddAsync(redisKey, 105.8600, 21.0320, $"driver_hn_2:{request.RideId}");
            await db.GeoAddAsync(redisKey, 105.8485, 21.0260, $"driver_hn_3:{request.RideId}");
            await db.GeoAddAsync(redisKey, 105.8700, 21.0400, $"driver_hn_4:{request.RideId}");
            await db.GeoAddAsync(redisKey, 105.8800, 21.0450, $"driver_hn_5:{request.RideId}");

            // Đà Nẵng
            await db.GeoAddAsync(redisKey, 108.2200, 16.0470, $"driver_dn_1:{request.RideId}");
            await db.GeoAddAsync(redisKey, 108.2300, 16.0500, $"driver_dn_2:{request.RideId}");
            await db.GeoAddAsync(redisKey, 108.2100, 16.0400, $"driver_dn_3:{request.RideId}");
            await db.GeoAddAsync(redisKey, 108.2400, 16.0600, $"driver_dn_4:{request.RideId}");
            await db.GeoAddAsync(redisKey, 108.2500, 16.0700, $"driver_dn_5:{request.RideId}");

            var geoRadiusResult = await db.GeoRadiusAsync(redisKey, request.PickupLongitude, 
                request.PickupLatitude, 5, GeoUnit.Kilometers);
            var driverstest = await db.ExecuteAsync("ZRANGE", redisKey, 0, -1);
            Console.WriteLine($"Drivers: {driverstest}");

            var nearbyDrivers = geoRadiusResult
                .Select(driver =>
                {
                    var parts = driver.Member.ToString().Split(':'); // Tách driverId và rideId
                    return new DriverInfo
                    {
                        DriverId = (parts[0]).ToString(),
                        RideId = Guid.Parse(parts[1]),
                        Distance = (double)(driver.Distance ?? 0)
                    };
                })
                .OrderBy(d => d.Distance)
                .ToList();

            Console.WriteLine($"Drivers: {nearbyDrivers}");
            
            return nearbyDrivers;
        }

    }
}
