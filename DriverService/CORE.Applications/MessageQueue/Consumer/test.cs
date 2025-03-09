//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System.Text;
//using System.Text.Json;
//using DriverService.Models;
//using DriverService.Services;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using CORE.Infrastructure.Repositories.Driver.Producer;
//using CORE.Applications.MessageQueue.Consumer;
//using CORE.Infrastructure.Shared.Models.Driver.Request;
//using CORE.Infrastructure.Repositories.Driver.Interfaces;

//namespace DriverService.Messaging
//{
//    public class DriverResponseConsumer : BackgroundService
//    {
//        private readonly IServiceScopeFactory _serviceScopeFactory;
//        private readonly IConfiguration _configuration;
//        private static Dictionary<Guid, List<DriverInfo>> rideDriverQueue = new(); // Lưu danh sách tài xế chờ

//        public DriverResponseConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//            _configuration = configuration;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var factory = new ConnectionFactory()
//            {
//                HostName = _configuration["RabbitMQ:Host"],
//                Port = int.Parse(_configuration["RabbitMQ:Port"]),
//                UserName = _configuration["RabbitMQ:Username"],
//                Password = _configuration["RabbitMQ:Password"]
//            };

//            using var connection = factory.CreateConnection();
//            using var channel = connection.CreateModel();
//            string queueName = _configuration["RabbitMQ:DriverResponseQueue"];

//            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
//            var consumer = new EventingBasicConsumer(channel);

//            consumer.Received += async (model, ea) =>
//            {
//                var body = ea.Body.ToArray();
//                var message = Encoding.UTF8.GetString(body);
//                var response = JsonSerializer.Deserialize<DriverResponse>(message);

//                if (response != null)
//                {
//                    using var scope = _serviceScopeFactory.CreateScope();
//                    var driverService = scope.ServiceProvider.GetRequiredService<IDriverCommandRepository>();
//                    var notificationService = scope.ServiceProvider.GetRequiredService<NotificationProducer>();

//                    if (response.Accepted)
//                    {
//                        await driverService.ConfirmDriverAssignment(response);
//                        Console.WriteLine($"✅ Tài xế {response.DriverId} đã nhận chuyến {response.RideId}");
//                        rideDriverQueue.Remove(response.RideId); // Xóa danh sách chờ
//                    }
//                    else
//                    {
//                        Console.WriteLine($"❌ Tài xế {response.DriverId} từ chối chuyến {response.RideId}");
//                        await AssignNextDriver(response.RideId, driverService, notificationService);
//                    }
//                }
//            };

//            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
//            await Task.Delay(Timeout.Infinite, stoppingToken);
//        }

//        private async Task AssignNextDriver(Guid rideId, IDriverService driverService, NotificationProducer notificationService)
//        {
//            if (!rideDriverQueue.ContainsKey(rideId) || rideDriverQueue[rideId].Count == 0)
//            {
//                Console.WriteLine($"⚠️ Không còn tài xế nào khả dụng cho chuyến {rideId}");
//                notificationService.SendNotification(rideId.ToString(), "Không có tài xế nào nhận chuyến, vui lòng thử lại sau.");
//                return;
//            }

//            var nextDriver = rideDriverQueue[rideId][0]; // Lấy tài xế tiếp theo
//            rideDriverQueue[rideId].RemoveAt(0); // Xóa tài xế khỏi danh sách

//            var driverRequest = new DriverAllocationRequest
//            {
//                RideId = rideId,
//                DriverId = nextDriver.DriverId,
//                PickupLatitude = nextDriver.PickupLatitude,
//                PickupLongitude = nextDriver.PickupLongitude
//            };

//            Console.WriteLine($"🚕 Gửi yêu cầu đến tài xế tiếp theo: {nextDriver.DriverId}");
//            driverService.SendDriverRequest(driverRequest);
//        }

//        public static void AddRideDrivers(Guid rideId, List<DriverInfo> drivers)
//        {
//            if (!rideDriverQueue.ContainsKey(rideId))
//            {
//                rideDriverQueue[rideId] = new List<DriverInfo>(drivers);
//            }
//        }
//    }
//}
