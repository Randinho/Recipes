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

namespace Recipes.Controllers
{
    public class NotificationController : BaseController
    {     
        private readonly ApplicationDbContext context;
        public NotificationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this.context = context;        
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await context.Notifications.Where(x => x.ReceiverId == GetCurrentUserId()).ToListAsync();

            return View(notifications);
        }

        public async Task<IActionResult> SetNotificationReceived(int? id)
        {
            if(id != null)
            {
              context.Notifications.FirstOrDefault(x => x.Id == id).IsReceived = true;
                
            }
            else
            {
                var notifications = await context.Notifications.Where(x => x.ReceiverId == GetCurrentUserId() && x.IsReceived == false).ToListAsync();
                foreach (var item in notifications) 
                {
                    item.IsReceived = true;
                    context.Notifications.Update(item);
                }
                    
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
