/*using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace CORE.Applications.Feature.Notification.MessageConsumer
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;

        public NotificationConsumer(IConfiguration configuration)
        {
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

            channel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"], durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"[✓] Received message: {message}");
            };

            channel.BasicConsume(queue: _configuration["RabbitMQ:QueueName"], autoAck: true, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
*/