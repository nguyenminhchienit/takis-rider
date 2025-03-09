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
using CORE.Infrastructure.Shared.Models;
using CORE.Infrastructure.Repositories.Noti.Interfaces;

namespace CORE.Applications.Feature.Notification.MessageConsumer
{
    public class NotificationFromDriveConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public NotificationFromDriveConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
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
            string queueName = _configuration["RabbitMQ:NotificationQueue"];

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var notification = JsonSerializer.Deserialize<NotificationMessage>(message);

                if (notification != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INoti>();
                    notificationService.SendNotification(notification.UserId, notification.Message);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
