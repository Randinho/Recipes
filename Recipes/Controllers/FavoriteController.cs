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

namespace Recipes.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        public ApplicationDbContext context { get; }
        public UserManager<ApplicationUser> userManager { get; }

        public FavoriteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        private string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            return userManager.GetUserId(currentUser);
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
            return RedirectToAction("Details", "Recipes", new { id });
        }

        public async Task<IActionResult> Delete(int recipeId, string userId)
        {
            var fav = await context.Favorites.FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);
            if(fav.ApplicationUserId == GetCurrentUserId())
            {
                context.Favorites.Remove(fav);
                await context.SaveChangesAsync();
            }
            

            return RedirectToAction(nameof(Index));
        }
    }
}
