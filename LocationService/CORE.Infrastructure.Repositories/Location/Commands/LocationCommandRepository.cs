
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Shared.Models.Location.Request;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CORE.Infrastructure.Repositories.Location.Commands
{
    public class LocationCommandRepository : ILocationCommandRepository
    {
        private const string GEO_KEY = "drivers:locations";
        private const string ONLINE_DRIVERS_KEY = "drivers:online"; // Danh sách tài xế online

        private readonly IConnectionMultiplexer _redis;

        public LocationCommandRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        /// <summary>
        /// Cập nhật vị trí tài xế nếu họ đã di chuyển đáng kể (hơn 50m)
        /// </summary>
        public async Task<bool> UpdateDriverLocationAsync(string driverId, Guid rideId, double latitude, double longitude)
        {
            string key = $"driver:{driverId}:location";
            string member = $"{driverId}:{rideId}";
            string lastLocation = await _redis.GetDatabase().StringGetAsync(key);

            if (!string.IsNullOrEmpty(lastLocation))
            {
                var lastCoords = lastLocation.Split(',');
                double lastLat = double.Parse(lastCoords[0]);
                double lastLon = double.Parse(lastCoords[1]);

                // Kiểm tra tài xế đã di chuyển hơn 50m chưa
                if (GetDistance(lastLat, lastLon, latitude, longitude) < 50)
                {
                    Console.WriteLine($"⚠️ Tài xế {driverId} di chuyển quá ít, không cần cập nhật.");
                    return false; // Không cập nhật nếu chưa di chuyển đủ 50m
                }
            }

            // Lưu vị trí mới vào Redis
            await _redis.GetDatabase().StringSetAsync(key, $"{latitude},{longitude}");
            await _redis.GetDatabase().GeoAddAsync(GEO_KEY, longitude, latitude, member);

            Console.WriteLine($"✅ Cập nhật vị trí tài xế {driverId}-{rideId}: ({latitude}, {longitude})");
            return true;
        }

        /// <summary>
        /// Tính khoảng cách giữa hai tọa độ (Haversine Formula)
        /// </summary>
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Bán kính Trái Đất (km)
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)) * 1000; // Trả về mét
        }


        /// <summary>
        /// Khi tài xế bật trạng thái "Online", thêm vào danh sách Redis
        /// </summary>
        public async Task SetDriverOnlineAsync(string driverId)
        {
            await _redis.GetDatabase().SetAddAsync(ONLINE_DRIVERS_KEY, driverId.ToString());
            Console.WriteLine($"✅ Tài xế {driverId} đã Online!");
        }

        /// <summary>
        /// Khi tài xế tắt trạng thái "Online", xóa khỏi danh sách
        /// </summary>
        public async Task SetDriverOfflineAsync(string driverId)
        {
            await _redis.GetDatabase().SetRemoveAsync(ONLINE_DRIVERS_KEY, driverId.ToString());
            Console.WriteLine($"❌ Tài xế {driverId} đã Offline!");
        }

        /// <summary>
        /// Lấy danh sách tài xế đang Online từ Redis
        /// </summary>
        public async Task<List<string>> GetOnlineDriversAsync()
        {
            var drivers = await _redis.GetDatabase().SetMembersAsync(ONLINE_DRIVERS_KEY);
            return drivers.Select(d => (d.ToString())).ToList();
        }

        public Task<bool> UpdateDriverLocationAsync(string driverId, double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
    }
}
