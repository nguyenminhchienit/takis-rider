using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CORE.Applications.MessageQueue
{
    public class UserProducer
    {

        private readonly IConfiguration _configuration;

        public UserProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendUserRegisteredEvent(object message)
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

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Console.WriteLine($"[✓] Sent user registered event: {JsonSerializer.Serialize(message)}");
        }
    }
}
