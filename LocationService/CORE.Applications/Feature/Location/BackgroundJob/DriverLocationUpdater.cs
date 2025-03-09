using CORE.Infrastructure.Repositories.Location.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace CORE.Applications.Feature.Location.BackgroundJob
{
    
    public class DriverLocationUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnectionMultiplexer _redis;

        public DriverLocationUpdater(IServiceScopeFactory serviceScopeFactory,IConnectionMultiplexer redis)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _redis = redis;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var locationService = scope.ServiceProvider.GetRequiredService<ILocationCommandRepository>();
                // Lấy danh sách tài xế đang Online
                var onlineDrivers = await locationService.GetOnlineDriversAsync();
                foreach (var driverId in onlineDrivers)
                {
                    // Giả lập lấy vị trí từ hệ thống khác hoặc từ mobile
                    //double latitude = 10.762622 + new Random().NextDouble() * 0.01;
                    //double longitude = 106.660172 + new Random().NextDouble() * 0.01;

                    string last_key_driver = $"driver:{driverId}:location";
                    string lastLocation = await _redis.GetDatabase().StringGetAsync(last_key_driver);

                    double latitude, longitude;

                    if (!string.IsNullOrEmpty(lastLocation))
                    {
                        var lastCoords = lastLocation.Split(',');
                        latitude = double.Parse(lastCoords[0]);
                        longitude = double.Parse(lastCoords[1]);
                    }
                    else
                    {
                        // Nếu không có dữ liệu, lấy vị trí từ ứng dụng mobile (Giả sử có API lấy vị trí)
                        //var locationData = await _locationService.FetchDriverLocationFromMobileAsync(driverId);
                        if (true) // kiem tra null
                        {
                            latitude = 10.762622 + new Random().NextDouble() * 0.01;
                            longitude = 106.660172 + new Random().NextDouble() * 0.01;
                        }
                        
                    }

                    await locationService.UpdateDriverLocationAsync(driverId, latitude, longitude);
                    Console.WriteLine($"📍 Cập nhật vị trí tài xế {driverId}");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Cập nhật mỗi 10 giây
            }
        }
    }
}
