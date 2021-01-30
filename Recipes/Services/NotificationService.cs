using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IMapper mapper,
            INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<NotificationDTO>> GetNotificationList(string userId)
        {
            var notifications = await _notificationRepository.GetUserNotifications(userId);
            var mapped = _mapper.Map<NotificationDTO[]>(notifications);
            return mapped;
        }
        public async Task SetNotificationsReceived(int? id, string userId)
        {
            if (id != null)
            {
                var notification = await _notificationRepository.GetNotificationById((int)id);
                await _notificationRepository.SetReceived(notification);
            }   
            else
            {
                var notifications = await _notificationRepository.GetAllNotReceivedNotifications(userId);
                foreach (var item in notifications)
                {
                    await _notificationRepository.SetReceived(item);
                }
            }
        }
        public async Task<bool> AnyNotReceivedNotification(string userId) => 
            await _notificationRepository.AnyNotReceivedNotification(userId);
            



    }
}
