using Recipes.Interfaces.Repositories;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Notification>> GetUserNotifications(string userId)
        {
            return await _context.Notifications.Where(x => x.ReceiverId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllNotReceivedNotifications(string userId)
        {
            return await _context.Notifications.Where(x => x.ReceiverId == userId && x.IsReceived == false).ToListAsync();
        }

        public async Task<Notification> GetNotificationById(int id){
            return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SetReceived(Notification item)
        {
            item.IsReceived = true;
            _context.Notifications.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyNotReceivedNotification(string userId) => 
            await _context.Notifications.AnyAsync(x => x.ReceiverId == userId && x.IsReceived == false);
        
            
        
    }
}
