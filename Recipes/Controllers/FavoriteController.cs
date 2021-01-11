using Microsoft.AspNetCore.Mvc;
using Recipes.Data;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Recipes.DTO;

namespace Recipes.Controllers
{
    [Authorize]
    public class FavoriteController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Favorite> _logger;

        public FavoriteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            ILogger<Favorite> logger, 
            IMapper mapper) : base(userManager, mapper)
        {        
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            
            var favoriteRecipes = await _context.Favorites
                .Include(x => x.Recipe.ApplicationUser)  
                .Include(x => x.Recipe.Category)
                .Where(x => x.ApplicationUserId == GetCurrentUserId()).ToListAsync();
                
            return View(_mapper.Map<FavoriteDTO[]>(favoriteRecipes));
        }

        public async Task<IActionResult> AddToFavorites(int id)
        {
            var Recipe = _context.Recipes.FirstOrDefault(x => x.Id == id);

            _context.Favorites.Add(new Favorite
            {
                ApplicationUserId = GetCurrentUserId(),
                RecipeId = id
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int recipeId)
        {
           
            var fav = await _context.Favorites.FirstOrDefaultAsync(x => x.ApplicationUserId == GetCurrentUserId() && x.RecipeId == recipeId); 
            if (fav != null)
            {
                _logger.LogInformation(fav.RecipeId.ToString());
                _logger.LogInformation(fav.ApplicationUserId);
                _context.Favorites.Remove(fav);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
