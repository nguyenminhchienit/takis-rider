
using CORE.Infrastructure.Repositories.Driver.Interfaces;
using CORE.Infrastructure.Repositories.Driver.Producer;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using CORE.Infrastructure.Shared.Models.Driver.Response;
using StackExchange.Redis;

namespace CORE.Infrastructure.Repositories.Driver.Commands
{
    public class DriverInfo
    {
        public string DriverId { get; set; } = string.Empty;
        public double Rating { get; set; }
        public double CancellationRate { get; set; }

        public Guid RideId { get; set; }
        public double Distance { get; set; }
    }
    public class DriverCommandRepository : IDriverCommandRepository
    {

        private readonly DbSqlContext dbSqlContext;
        private readonly IConnectionMultiplexer redis;

        private readonly LocationRequestProducer _locationProducer;
        private readonly DriverRequestProducer _driverProducer;
        private readonly NotificationProducer _notificationProducer;
        private const int MaxRetries = 3;
        private Dictionary<Guid, int> rideRetryCount = new();

        public DriverCommandRepository(DbSqlContext context, IConnectionMultiplexer _redis, LocationRequestProducer locationProducer,
            NotificationProducer notificationProducer, DriverRequestProducer driverProducer)
        {
            dbSqlContext = context;
            redis = _redis;
            _locationProducer = locationProducer;
            _notificationProducer = notificationProducer;
            _driverProducer = driverProducer;
        }

        public async Task<bool> FindAndAssignDriverAsyncNew(Dictionary<Guid, List<DriverInfo>> rideDriverQueue) //rideDriverQueue la ds driver doi
        {
            foreach (var rideEntry in rideDriverQueue)
            {
                var rideId = rideEntry.Key;
                var availableDrivers = rideEntry.Value;

                if (availableDrivers == null || availableDrivers.Count == 0)
                {
                    _notificationProducer.SendNotification(rideId.ToString(), "⚠️ Không có tài xế nào khả dụng. Vui lòng thử lại sau.");
                    return false;
                }

                // Sắp xếp danh sách tài xế theo khoảng cách gần nhất (nếu có)
                availableDrivers = availableDrivers.OrderBy(d => d.Distance).ToList();

                if (!rideDriverQueue.ContainsKey(rideId))
                {
                    rideDriverQueue[rideId] = new List<DriverInfo>(availableDrivers);
                }

                // Gửi thông báo và yêu cầu đến tài xế đầu tiên
                await NotifyAndAssignNextDriver(rideId, rideDriverQueue);
            }

            return true;
        }

        private async Task NotifyAndAssignNextDriver(Guid rideId, Dictionary<Guid, List<DriverInfo>> rideDriverQueue)
        {
            if (!rideDriverQueue.ContainsKey(rideId) || rideDriverQueue[rideId].Count == 0)
            {
                Console.WriteLine($"⚠️ Không còn tài xế nào khả dụng cho chuyến {rideId}");
                _notificationProducer.SendNotification(rideId.ToString(), "❌ Không có tài xế nào nhận chuyến. Vui lòng thử lại sau.");
                return;
            }

            var nextDriver = rideDriverQueue[rideId][0]; // Lấy tài xế tiếp theo
            rideDriverQueue[rideId].RemoveAt(0); // Xóa khỏi danh sách chờ

            // Gửi thông báo trước
            _notificationProducer.SendNotification(nextDriver.DriverId, $"🚖 Bạn có một yêu cầu chuyến đi mới ({rideId}). Chấp nhận?");

            Console.WriteLine($"📩 Gửi thông báo đến tài xế {nextDriver.DriverId}");

            await Task.Delay(3000); // Chờ 3 giây cho tài xế xem xét

            var driverRequest = new DriverAllocationRequest
            {
                RideId = rideId,
                DriverId = nextDriver.DriverId,
                //PickupLatitude = nextDriver.PickupLatitude,
                //PickupLongitude = nextDriver.PickupLongitude
            };

            Console.WriteLine($"📤 Gửi yêu cầu đến tài xế: {nextDriver.DriverId}");
            //_driverProducer.SendDriverRequest(driverRequest);
        }

        public async Task<bool> FindAndAssignDriverAsync(RideAllocationRequest request)
        {
            if (!rideRetryCount.ContainsKey(request.RideId))
                rideRetryCount[request.RideId] = 0;

            var availableDrivers = await _locationProducer.RequestNearbyDrivers(request);

            if (availableDrivers.Count == 0)
            {
                _notificationProducer.SendNotification(request.UserId, "Không có tài xế nào khả dụng.");
                return false;
            }

            //foreach (var driver in availableDrivers)
            //{
            //    _driverProducer.SendDriverRequest(new DriverAllocationRequest
            //    {
            //        RideId = request.RideId,
            //        DriverId = driver.DriverId,
            //        PickupLatitude = request.PickupLatitude,
            //        PickupLongitude = request.PickupLongitude
            //    });

            //    return true;
            //}

            return false;
        }

        
        public void ConfirmDriverAssignment(DriverAllocationResponse response)
        {
            _notificationProducer.SendNotification(response.DriverId, "Bạn đã nhận chuyến.");
            _notificationProducer.SendNotification(response.RideId.ToString(), "Tài xế đã nhận chuyến.");
            rideRetryCount.Remove(response.RideId);
        }

        

        public async Task<DriverModel> RegisterDriverAsync(DriverModelRequest request)
        {
            var driver = new DriverModel
            {
                Id = request.UserId,
                Name = request.Name,
                VehicleType = request.VehicleType,
                LicensePlate = request.LicensePlate,
                Status = DriverStatus.Offline
            };

            await dbSqlContext.Drivers.AddAsync(driver);
            await dbSqlContext.SaveChangesAsync();

            return driver;
        }

        public async Task<bool> UpdateDriverStatusAsync(string driverId, DriverStatus status)
        {
            var driver = await dbSqlContext.Drivers.FindAsync(driverId);
            if (driver == null) return false;

            driver.Status = status;
            await dbSqlContext.SaveChangesAsync();

            var db = redis.GetDatabase();
            await db.StringSetAsync($"driver:{driverId}:status", status.ToString());

            return true;
        }

        public Task AssignNextDriver(Guid rideId)
        {
            throw new NotImplementedException();
        }
    }
}
