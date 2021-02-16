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
        private readonly IUserService _userService;

        public ShareController(UserManager<ApplicationUser> userManager,
            IShareService shareService,
            IUserService userService) : base(userManager)
        {
            _shareService = shareService;
            _userService = userService;
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
            string message;
            var user = await _userService.CheckIfUserExists(email);
            if (user == null)
            {
                message = "User with this email does not exist.";
                return RedirectToAction("PermissionDenied", "Home", new { message });
            }

            if (await _shareService.ShareRecipe(id, user))
                return RedirectToAction("UserRecipes", "Recipes");
            else
            {
                message = "That recipe is already shared to chosen user.";
                return RedirectToAction("PermissionDenied", "Home", new { message });
            }
        }
    }
}
