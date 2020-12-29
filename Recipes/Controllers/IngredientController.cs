using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class IngredientController : Controller
    {
        public ApplicationDbContext context { get; }
        public ILogger<Ingredient> Logger { get; }

        public IngredientController(ApplicationDbContext context, ILogger<Ingredient> logger)
        {
            this.context = context;
            Logger = logger;
        }
        [HttpGet]
        public IActionResult Source(string term)
        {
            //string term = HttpContext.Request.Query["term"].ToString();
            var result = (from I in context.Ingredients
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
                    Logger.LogInformation("Dodawanie składnika.");
                    context.Ingredients.Add(new Ingredient
                    {
                        Name = ingredientName
                    });
                    await context.SaveChangesAsync();
                }
                var ingredient = await context.Ingredients.FirstOrDefaultAsync(x => x.Name == ingredientName);
                await context.RecipeIngredients.AddAsync(new RecipeIngredients
                {
                    RecipeId = recipeId,
                    IngredientId = ingredient.Id,
                    Amount = amount
                });
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Edit", "Recipes", new { id = recipeId });
        }

        public async Task<IActionResult> Delete(int ingredientId, int recipeId)
        {
            var ingredient = await context.RecipeIngredients.FirstOrDefaultAsync(x => x.IngredientId == ingredientId && x.RecipeId == recipeId);
            context.RecipeIngredients.Remove(ingredient);
            await context.SaveChangesAsync();

            return RedirectToAction("Edit", "Recipes", new { id = recipeId });
        }

        private bool IngredientExists(string name)
        {
            Logger.LogInformation(context.Ingredients.Any(x => x.Name == name).ToString());
            return context.Ingredients.Any(x => x.Name == name);
        }
    }
}
