using Microsoft.AspNetCore.Mvc;
using Recipes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Recipes.Controllers
{
    [Authorize]
    public class FavoriteController : BaseController
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<Favorite> logger;

        public FavoriteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<Favorite> logger) : base(userManager)
        {        
            this.context = context;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            
            var favoriteRecipes = await context.Favorites
                .Include(x => x.Recipe.ApplicationUser)  
                .Include(x => x.Recipe.Category)
                .Where(x => x.ApplicationUserId == GetCurrentUserId()).ToListAsync();
                
            return View(favoriteRecipes);
        }

        public async Task<IActionResult> AddToFavorites(int id)
        {
            var Recipe = context.Recipes.FirstOrDefault(x => x.Id == id);

            context.Favorites.Add(new Favorite
            {
                ApplicationUserId = GetCurrentUserId(),
                RecipeId = id
            });

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int recipeId)
        {
           
            var fav = await context.Favorites.FirstOrDefaultAsync(x => x.ApplicationUserId == GetCurrentUserId() && x.RecipeId == recipeId); 
            if (fav != null)
            {
                logger.LogInformation(fav.RecipeId.ToString());
                logger.LogInformation(fav.ApplicationUserId);
                context.Favorites.Remove(fav);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
