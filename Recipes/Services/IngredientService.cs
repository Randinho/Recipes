using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.Interfaces;
using Recipes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IngredientService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Create(string ingredientName, string amount, int recipeId)
        {
            if (!IngredientExists(ingredientName))
            {
                await _context.AddAsync(new Ingredient
                {
                    Name = ingredientName
                });
                await _context.SaveChangesAsync();
            }
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(x => x.Name == ingredientName);
            await _context.RecipeIngredients.AddAsync(new RecipeIngredients
            {
                RecipeId = recipeId,
                IngredientId = ingredient.Id,
                Amount = amount
            });
            await _context.SaveChangesAsync();
        }
        public async Task Remove(int ingredientId, int recipeId)
        {
            var ingredient = await _context.RecipeIngredients
                .FirstOrDefaultAsync(x => x.IngredientId == ingredientId && x.RecipeId == recipeId);
            _context.RecipeIngredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

        private bool IngredientExists(string name) => _context.Ingredients.Any(x => x.Name == name);
    }
}
