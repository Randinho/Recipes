using Recipes.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationList(string userId);
        Task SetNotificationsReceived(int? id, string userId);
    }
}
