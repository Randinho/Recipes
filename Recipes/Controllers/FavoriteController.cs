using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Models;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Authorize]
    public class FavoriteController : BaseController
    {
        private readonly ILogger<Favorite> _logger;
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(UserManager<ApplicationUser> userManager,
            ILogger<Favorite> logger,
            IFavoriteService favoriteService) : base(userManager)
        {
            _logger = logger;
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var favoriteRecipes = await _favoriteService.GetFavoriteList(GetCurrentUserId());
            int pageSize = 12;
            return View(PaginatedList<FavoriteDTO>.Create(favoriteRecipes, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> AddToFavorites(int id)
        {
            await _favoriteService.Add(id, GetCurrentUserId());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int recipeId)
        {
            await _favoriteService.Remove(recipeId, GetCurrentUserId());
            return RedirectToAction(nameof(Index));
        }

    }
}
