using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        public ApplicationDbContext _context { get; }
        public ILogger<Ingredient> _logger { get; }

        public IngredientController(ApplicationDbContext context, ILogger<Ingredient> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult IngredientSource(string term)
        {
            //string term = HttpContext.Request.Query["term"].ToString();
            var result = (from I in _context.Ingredients
                          where I.Name.Contains(term)
                          select new { value = I.Name });
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> AddIngredient(string ingredientName, string amount, int recipeId)
        {
            if (ModelState.IsValid)
            {
                if (IngredientExists(ingredientName) == false)
                {
                    _logger.LogInformation("Dodawanie składnika.");
                    _context.Ingredients.Add(new Ingredient
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

            return RedirectToAction("Edit", "Recipes", new { id = recipeId });
        }

        public async Task<IActionResult> Delete(int ingredientId, int recipeId)
        {
            var ingredient = await _context.RecipeIngredients.FirstOrDefaultAsync(x => x.IngredientId == ingredientId && x.RecipeId == recipeId);
            _context.RecipeIngredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Recipes", new { id = recipeId });
        }

        private bool IngredientExists(string name) => _context.Ingredients.Any(x => x.Name == name);   
    }
}
