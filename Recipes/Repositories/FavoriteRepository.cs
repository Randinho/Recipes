using Recipes.Interfaces.Repositories;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;
        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Favorite> GetFavoriteRecipe(string userId, int recipeId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.ApplicationUserId == userId && f.RecipeId == recipeId);
        }
        public async Task AddToFavorites(string userId, int recipeId)
        {
            await _context.Favorites.AddAsync(new Favorite
            {
                ApplicationUserId = userId,
                RecipeId = recipeId
            });
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Favorite>> GetFavoriteList(string userId)
        {
            var favoriteRecipes = await _context.Favorites
               .Include(x => x.Recipe.ApplicationUser)
               .Include(x => x.Recipe.Category)
               .Where(x => x.ApplicationUserId == userId).ToListAsync();
            return favoriteRecipes;
        }
        public async Task Remove(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

        }

    }
}
