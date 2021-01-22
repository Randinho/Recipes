namespace Recipes.Interfaces
{
    public interface INotificationSender
    {
        void SendNotification(string message, string userId);
    }
}
