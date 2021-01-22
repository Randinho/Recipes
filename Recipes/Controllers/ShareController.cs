using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Models;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Authorize]
    public class ShareController : BaseController
    {
        private readonly IShareService _shareService;
        
        public ShareController(UserManager<ApplicationUser> userManager, 
            IShareService shareService) : base(userManager)
        {
            _shareService = shareService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var shared = await _shareService.GetSharedRecipesList(GetCurrentUserId());

            int pageSize = 12;
            return View(PaginatedList<SharedDTO>.Create(shared, pageNumber ?? 1, pageSize));
        }
     
        public IActionResult Share(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> Share(int id, string email)
        {

            if (await _shareService.ShareRecipe(id, email))
                return RedirectToAction("UserRecipes", "Recipes");
            else
            {
                string message = "That recipe is already shared to chosen user.";
                return RedirectToAction("PermissionDenied", "Home", new { message });
            }
        }
    }
}
