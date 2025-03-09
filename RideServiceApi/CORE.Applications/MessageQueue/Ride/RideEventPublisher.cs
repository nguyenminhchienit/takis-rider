using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CORE.Applications.MessageQueue.Ride
{
    public class RideEventPublisher
    {
        private readonly IModel _channel;

        public RideEventPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "notification_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishDriverArrivedEvent(Guid rideId)
        {
            var message = new { RideId = rideId, Event = "DriverArrived" };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(exchange: "", routingKey: "notification_queue", basicProperties: null, body: body);
        }
    }
}
