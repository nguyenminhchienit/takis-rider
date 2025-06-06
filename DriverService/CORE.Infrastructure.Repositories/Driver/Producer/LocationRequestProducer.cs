﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Driver.Commands;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CORE.Infrastructure.Repositories.Driver.Producer
{
    public class LocationRequestProducer
    {
        private readonly IConfiguration _configuration;

        public LocationRequestProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<DriverInfo>> RequestNearbyDrivers(RideAllocationRequest request)
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
            string queueName = _configuration["RabbitMQ:LocationRequestQueue"];

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var message = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            return await Task.FromResult(new List<DriverInfo>
            {
                new DriverInfo { DriverId = "driver_1", Rating = 4.8 },
                new DriverInfo { DriverId = "driver_2", Rating = 4.5 }
            });
        }

    }
}
