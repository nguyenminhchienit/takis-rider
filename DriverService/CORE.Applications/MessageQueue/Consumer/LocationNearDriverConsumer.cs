using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Driver.Commands;
using CORE.Infrastructure.Repositories.Driver.Interfaces;
using CORE.Infrastructure.Repositories.Driver.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CORE.Applications.MessageQueue.Consumer
{
    
    public class LocationNearDriverConsumer : BackgroundService
    {

        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;
        private static Dictionary<Guid, List<DriverInfo>> rideDriverQueue = new(); // Lưu danh sách tài xế chờ


        public LocationNearDriverConsumer(IServiceScopeFactory _serviceScopeFactory, IConfiguration _configuration)
        {
            serviceScopeFactory = _serviceScopeFactory;
            configuration = _configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:Host"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:Username"],
                Password = configuration["RabbitMQ:Password"]
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            string queueName = configuration["RabbitMQ:NotiDriverResponseQueue"];

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                List<DriverInfo> notiResponse = JsonSerializer.Deserialize<List<DriverInfo>>(message);
                Console.WriteLine($"📤 Nhan phan hoi tu noti");
                if (notiResponse != null) {
                    rideDriverQueue[notiResponse.First().RideId] = notiResponse;

                    using var scope = serviceScopeFactory.CreateScope();
                    var driverService = scope.ServiceProvider.GetRequiredService<IDriverCommandRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<NotificationProducer>();

                    driverService.FindAndAssignDriverAsyncNew(rideDriverQueue);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
