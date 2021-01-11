using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.AspNetCore.Identity;
using Recipes.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Recipes.DTO;

namespace Recipes.Controllers
{
    public class NotificationController : BaseController
    {     
        private readonly ApplicationDbContext _context;
        public NotificationController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper) : base(userManager, mapper)
        {
            _context = context;        
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await _context.Notifications.Where(x => x.ReceiverId == GetCurrentUserId()).ToListAsync();

            return View(_mapper.Map<NotificationDTO>(notifications));
        }

        public async Task<IActionResult> SetNotificationReceived(int? id)
        {
            if(id != null)
            {
              _context.Notifications.FirstOrDefault(x => x.Id == id).IsReceived = true;
                
            }
            else
            {
                var notifications = await _context.Notifications.Where(x => x.ReceiverId == GetCurrentUserId() && x.IsReceived == false).ToListAsync();
                foreach (var item in notifications) 
                {
                    item.IsReceived = true;
                    _context.Notifications.Update(item);
                }
                    
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
