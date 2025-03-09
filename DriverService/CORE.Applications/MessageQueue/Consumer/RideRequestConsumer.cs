using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using CORE.Infrastructure.Repositories.Driver.Interfaces;
using MediatR;

namespace CORE.Applications.MessageQueue.Consumer
{
    public class RideRequestConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public RideRequestConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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
            string queueName = _configuration["RabbitMQ:RideRequestQueue"];

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var rideRequest = JsonSerializer.Deserialize<RideAllocationRequest>(message);
                Console.WriteLine($"📤 Nhan yeu cau tu ride request");

                if (rideRequest != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var driverService = scope.ServiceProvider.GetRequiredService<IDriverCommandRepository>();
                    await driverService.FindAndAssignDriverAsync(rideRequest);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
            
        }
    }
}
