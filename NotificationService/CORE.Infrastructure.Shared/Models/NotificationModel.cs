using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CORE.Infrastructure.Shared.Models
{
    public class NotificationModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Type { get; set; } = string.Empty; // Email, SMS, Push
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
