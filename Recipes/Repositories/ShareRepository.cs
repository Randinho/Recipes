using Recipes.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes.Repositories
{
    public class ShareRepository : IShareRepository
    {
        private readonly ApplicationDbContext _context;
        public ShareRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Shared>> GetSharedRecipesList(string userId)
        {
            var shared = await _context.Shared
                 .Include(s => s.Recipe.ApplicationUser)
                 .Include(s => s.Recipe.Category)
                 .Where(s => s.ApplicationUserId == userId).ToListAsync();
            return shared;
        }
        public async Task<bool> IsAlreadyShared(int recipeId, string userId) =>
            await _context.Shared.AnyAsync(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);
        public async Task AddShared(string userId, int recipeId)
        {
            await _context.AddAsync(new Shared
            {
                RecipeId = recipeId,
                ApplicationUserId = userId,
                Confirmed = true
            });
            await _context.SaveChangesAsync();
        }

        
    }
}
