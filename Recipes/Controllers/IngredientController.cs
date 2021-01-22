using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Interfaces;
using Recipes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Ingredient> _logger;
        private readonly IIngredientService _ingredientService;

        public IngredientController(ApplicationDbContext context,
            ILogger<Ingredient> logger,
            IIngredientService ingredientService)
        {
            _context = context;
            _logger = logger;
            _ingredientService = ingredientService;
        }
        [HttpGet]
        public IActionResult IngredientSource(string term)
        {
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
                await _ingredientService.Create(ingredientName, amount, recipeId);
            }
            return RedirectToAction("Edit", "Recipes", new { id = recipeId });
        }

        public async Task<IActionResult> Delete(int ingredientId, int recipeId)
        {
            await _ingredientService.Remove(ingredientId, recipeId);

            return RedirectToAction("Edit", "Recipes", new { id = recipeId });
        }
    }
}
