using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.Models;
using Recipes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class ShareController : Controller
    {
        public ApplicationDbContext context { get; }
        public UserManager<ApplicationUser> userManager { get; }
        public NotificationSender notificationSender;
        
        public ShareController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            notificationSender = new NotificationSender(context);
        }

        public async Task<IActionResult> Index()
        {
            var shared = await context.Shared
                .Include(x => x.Recipe.ApplicationUser)
                .Include(x => x.Recipe.Category)
                .Where(x => x.ApplicationUserId == userManager.GetUserId(this.User)).ToListAsync();
            return View(shared);
        }
     
        public IActionResult Share(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> Share(int id, string email)
        {
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
            context.Shared.Add(new Shared
            {
                RecipeId = id,
                ApplicationUserId = user.Id,
                Confirmed = true
            });
            notificationSender.SendNotification("You received a recipe. Check it out in 'shared with me' tab.", user.Id);
            await context.SaveChangesAsync();
            return RedirectToAction("UserRecipes", "Recipes");
        }
    }
}
