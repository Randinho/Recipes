using Recipes.Data;
using Recipes.Interfaces;
using Recipes.Models;
using System;

namespace Recipes.Services
{
    class NotificationSender : INotificationSender
    {
        private readonly ApplicationDbContext _context;

        public NotificationSender(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SendNotification(string message, string userId)
        {
            var notification = new Notification
            {
                Message = message,
                ReceiverId = userId,
                Date = DateTime.Now,
                IsReceived = false
            };
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
    }
}
