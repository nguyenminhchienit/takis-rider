
using CORE.Infrastructure.Repositories.Driver.Interfaces;
using CORE.Infrastructure.Repositories.Driver.Producer;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using CORE.Infrastructure.Shared.Models.Driver.Response;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace CORE.Infrastructure.Repositories.Driver.Commands
{

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

        public async Task<bool> FindAndAssignDriverAsyncNew(Dictionary<Guid, List<DriverInfo>> rideDriverQueue)
        {
            foreach (var rideEntry in rideDriverQueue)
            {
                var rideId = rideEntry.Key;
                var availableDrivers = rideEntry.Value;

                if (availableDrivers == null || availableDrivers.Count == 0)
                {
                    //_notificationProducer.SendNotification(rideId.ToString(), "⚠️ Không có tài xế nào khả dụng. Vui lòng thử lại sau.");
                    continue;
                }

                // Sắp xếp danh sách tài xế theo khoảng cách gần nhất
                rideDriverQueue[rideId] = availableDrivers.OrderBy(d => d.Distance).ToList();

                // Lặp qua tất cả tài xế của chuyến đi này
                await NotifyAllDrivers(rideId, rideDriverQueue[rideId]);
            }

            return true;
        }

        private async Task NotifyAllDrivers(Guid rideId, List<DriverInfo> driverList)
        {
            Console.WriteLine($"📩 Gửi thông báo đến tất cả tài xế cho chuyến {rideId}");

            var notificationMessage = new NotiObjList
            {
                rideDriverQueue = driverList,
                RideId = rideId,
                Message = "Bạn có yêu cầu chuyến đi mới"
            };

            // Gửi một lần với danh sách tất cả tài xế
            await Task.Run(() =>
            {
                _notificationProducer.SendNotification("Thong Bao", notificationMessage);
            });

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

        public async Task<bool> FindAndAssignDriverAsync(RideAllocationRequest request)
        {
            if (!rideRetryCount.ContainsKey(request.RideId))
                rideRetryCount[request.RideId] = 0;

            var availableDrivers = await _locationProducer.RequestNearbyDrivers(request);

            if (availableDrivers.Count == 0)
            {
                //_notificationProducer.SendNotification(request.UserId, "Không có tài xế nào khả dụng.");
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
            //_notificationProducer.SendNotification(response.DriverId, "Bạn đã nhận chuyến.");
            //_notificationProducer.SendNotification(response.RideId.ToString(), "Tài xế đã nhận chuyến.");
            rideRetryCount.Remove(response.RideId);
        }

    }
}

