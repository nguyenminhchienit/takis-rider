using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace CORE.Applications.Feature.Notification.Services
{
    public class PushNotificationService
    {
        public PushNotificationService()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("path/to/firebase-key.json")
            });
        }

        public async Task SendPushNotification(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
