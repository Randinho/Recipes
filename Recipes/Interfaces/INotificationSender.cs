using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface INotificationSender
    {
        void SendNotification(string message, string userId);
    }
}
