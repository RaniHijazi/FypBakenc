namespace Fyp.Repository
{
    using FirebaseAdmin.Messaging;
    using Fyp.Interfaces;
    using System.Threading.Tasks;

    public class FcmService : IFcmService
    {
        public async Task<string> SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
    }

}
