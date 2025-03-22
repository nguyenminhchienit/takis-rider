using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CORE.Infrastructure.Repositories.Driver.Producer
{
    public class NotificationProducer
    {
        private readonly IConfiguration _configuration;

        public NotificationProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendNotification(string userId, string message)
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
            var notification = JsonSerializer.Serialize(new NotificationMessage { UserId = userId, Message = message });
            var body = Encoding.UTF8.GetBytes(notification);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }

    public class NotificationMessage
    {
        public string UserId { get; set; } = string.Empty;   // ID của người nhận thông báo
        public string Message { get; set; } = string.Empty; // Nội dung thông báo
    }
}
