using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetUserNotifications(string userId);
        Task<Notification> GetNotificationById(int id);
        Task<IEnumerable<Notification>> GetAllNotReceivedNotifications(string userId);
        Task SetReceived(Notification item);
        Task<bool> AnyNotReceivedNotification(string userId);
    }
}
