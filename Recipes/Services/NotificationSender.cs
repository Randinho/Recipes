using Recipes.Data;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class NotificationSender
    {
        public NotificationSender(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ApplicationDbContext context { get; }

        public void SendNotification(string message, string userId)
        {
            var notification = new Notification
            {
                Message = message,
                ReceiverId = userId,
                Date = DateTime.Now,
                IsReceived = false
            };
            context.Notifications.Add(notification);
            context.SaveChanges();
        }
    }
}
