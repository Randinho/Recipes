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
    public class IngredientRepository : IIngredientRepository
    {
        private readonly ApplicationDbContext _context;
        public IngredientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddIngredient(string name)
        {
            if (!IngredientExists(name))
            {
                await _context.AddAsync(new Ingredient
                {
                    Name = name
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddIngredientToRecipe(int ingredientId, int recipeId, string amount)
        {
            await _context.RecipeIngredients.AddAsync(new RecipeIngredients
            {
                RecipeId = recipeId,
                IngredientId = ingredientId,
                Amount = amount
            });
            await _context.SaveChangesAsync();
        }

        public async Task<Ingredient> GetIngredientByName(string name)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task RemoveIngredientFromRecipe(int ingredientId, int recipeId)
        {
            var recipeIngredient = await _context.RecipeIngredients
                 .FirstOrDefaultAsync(x => x.IngredientId == ingredientId && x.RecipeId == recipeId);
            _context.RecipeIngredients.Remove(recipeIngredient);
            await _context.SaveChangesAsync();
        }

        private bool IngredientExists(string name) => _context.Ingredients.Any(x => x.Name == name);

    }
}
