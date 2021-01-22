using Recipes.DTO;
using Recipes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotificationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NotificationDTO>> GetNotificationList(string userId)
        {
            var notifications = await _context.Notifications.Where(n => n.ReceiverId == userId).ToListAsync();
            var mapped = _mapper.Map<NotificationDTO[]>(notifications);
            return mapped;
        }
        public async Task SetNotificationsReceived(int? id, string userId)
        {
            if (id != null)
                _context.Notifications.FirstOrDefault(x => x.Id == id).IsReceived = true;
            else
            {
                var notifications = await _context.Notifications.Where(x => x.ReceiverId == userId && x.IsReceived == false).ToListAsync();
                foreach(var item in notifications)
                {
                    item.IsReceived = true;
                    _context.Notifications.Update(item);
                }
            }
            await _context.SaveChangesAsync();
            
        }
    }
}
