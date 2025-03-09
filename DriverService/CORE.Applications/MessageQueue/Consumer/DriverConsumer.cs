using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using System.Text.Json;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using System.Threading.Channels;

namespace CORE.Applications.MessageQueue.Consumer
{
    public class DriverConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public DriverConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
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

            string queueName = "user_registered_queue";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DbSqlContext>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var user = JsonSerializer.Deserialize<DriverModelRequest>(message);

                
                    var driver = new DriverModel
                    {
                        Id = user.UserId, // Lấy UserId từ UserServic
                        Name = user.Name,
                        VehicleType = user.VehicleType,
                        LicensePlate = user.LicensePlate
                    };

                    dbContext.Drivers.Add(driver);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine($"[✓] Registered driver for user: {user.Name}");
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
