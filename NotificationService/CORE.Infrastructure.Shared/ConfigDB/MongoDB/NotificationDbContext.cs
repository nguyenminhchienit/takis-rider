using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Shared.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CORE.Infrastructure.Shared.ConfigDB.MongoDB
{
    public class NotificationDbContext
    {
        private readonly IMongoDatabase _database;

        public NotificationDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<NotificationModel> Notifications => _database.GetCollection<NotificationModel>("Notifications");

        //public void SeedData()
        //{
        //    SeedNotifications();
        //}

        //private void SeedNotifications()
        //{
        //    var notificationCollection = Notifications;

            
            
        //    var fakeNotifications = new List<NotificationModel>
        //    {
        //        new NotificationModel { Id = ObjectId.GenerateNewId(), Type = "Email", Recipient = "alice@example.com", Message = "Welcome to our service!", SentAt = DateTime.UtcNow },
        //        new NotificationModel { Id = ObjectId.GenerateNewId(), Type = "SMS", Recipient = "+1234567890", Message = "Your OTP is 123456", SentAt = DateTime.UtcNow },
        //        new NotificationModel { Id = ObjectId.GenerateNewId(), Type = "Push", Recipient = "Device_001", Message = "New promotion available!", SentAt = DateTime.UtcNow }
        //    };

        //    notificationCollection.InsertMany(fakeNotifications);
            
        //}
    }
}
