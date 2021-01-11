using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Models;
using Recipes.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Authorize]
    public class ShareController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private NotificationSender notificationSender;
        
        public ShareController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper) : base(userManager, mapper)
        {
            _context = context;
            notificationSender = new NotificationSender(context);
        }

        public async Task<IActionResult> Index()
        {
            var shared = await _context.Shared
                .Include(x => x.Recipe.ApplicationUser)
                .Include(x => x.Recipe.Category)
                .Where(x => x.ApplicationUserId == GetCurrentUserId()).ToListAsync();
            return View(_mapper.Map<SharedDTO[]>(shared));
        }
     
        public IActionResult Share(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> Share(int id, string email)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (!IsAlreadyShared(id, user.Id))
            {
                _context.Shared.Add(new Shared
                {
                    RecipeId = id,
                    ApplicationUserId = user.Id,
                    Confirmed = true
                });
                notificationSender.SendNotification("You received a recipe. Check it out in 'shared with me' tab.", user.Id);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserRecipes", "Recipes");
            }
            else
            {
                string message = "That recipe is already shared to chosen user.";
                return RedirectToAction("PermissionDenied", "Home", new { message });
            }
        }

        private bool IsAlreadyShared(int recipeId, string userId) => _context.Shared.Any(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);
        
    }
}
