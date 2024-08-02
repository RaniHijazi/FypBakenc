namespace Fyp.Interfaces
{
    public interface IFcmService
    {
        Task<string> SendNotificationAsync(string token, string title, string body);
    }

}
