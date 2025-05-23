﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models.Ride.Request;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CORE.Applications.MessageQueue.Ride
{
    public class RideProducer
    {
        private readonly IConfiguration _configuration;

        public RideProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendNotification(object message)
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

            string queueName = _configuration["RabbitMQ:QueueName"];
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            Console.WriteLine($"[✓] Sent notification: {JsonSerializer.Serialize(message)}");
        }

        public void SendRideRequest(RideAllocationRequest request)
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
            var message = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Console.WriteLine($"📤 Gửi yêu cầu đặt xe SendRideRequest: {request.RideId} từ {request.UserId}");
        }
    }
}
