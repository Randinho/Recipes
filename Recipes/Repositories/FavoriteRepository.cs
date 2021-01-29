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
    }
}
