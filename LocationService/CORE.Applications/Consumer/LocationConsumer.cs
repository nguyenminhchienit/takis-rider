using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Shared.Models.Location.Request;
using MediatR;

namespace CORE.Applications.Consumer
{
    public class LocationConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public LocationConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            string queueName = _configuration["RabbitMQ:DriverRequestQueue"];
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var rideRequest = JsonSerializer.Deserialize<RideAllocationRequest>(message);
                Console.WriteLine($"📤 Nhan yêu cầu tu Drive DriverRequestQueue:cho chuyen {rideRequest.RideId}");

                if (rideRequest != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var locationService = scope.ServiceProvider.GetRequiredService<ILocationQueryRepository>();

                    var nearbyDrivers = await locationService.GetNearbyDrivers(rideRequest);

                    if(nearbyDrivers != null)
                    {

                    }

                    // Gửi phản hồi danh sách tài xế về Driver Service
                    var responseQueue = _configuration["RabbitMQ:NotiDriverResponseQueue"]; // ben driver service chua consumer ( con thieu nho bo sung ) xong roi gui lan luot cho cac tai xe
                    using var responseChannel = connection.CreateModel();
                    var responseMessage = JsonSerializer.Serialize(nearbyDrivers);
                    var responseBody = Encoding.UTF8.GetBytes(responseMessage);

                    responseChannel.BasicPublish(exchange: "", routingKey: responseQueue, basicProperties: null, body: responseBody);
                }
            };


            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
