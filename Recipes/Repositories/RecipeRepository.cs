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
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Recipe>> GetRecipeList()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.ApplicationUser)
                .Where(r => r.IsPrivate == false).ToListAsync();
            return recipes;
        }
        public async Task<Recipe> GetRecipeById(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.ApplicationUser)
                .Include(r => r.RecipeIngredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id);
            return recipe;
        }
        public async Task<IEnumerable<Recipe>> GetUserRecipes(string userId)
        {
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .Where(r => r.ApplicationUserId == userId).ToListAsync();
            return recipes;
        } 
        public async Task Create(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }
        public async Task Remove(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            _context.Remove(recipe);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> RecipeExists(int id) =>
            await _context.Recipes.AnyAsync(x => x.Id == id);
    }
}
