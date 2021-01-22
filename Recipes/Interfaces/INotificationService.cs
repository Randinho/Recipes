using Recipes.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationList(string userId);
        Task SetNotificationsReceived(int? id, string userId);
        Task<bool> AnyNotReceivedNotification(string userId);
    }
}
