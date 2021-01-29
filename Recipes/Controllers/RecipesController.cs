using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Models;
using Recipes.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{

    public class RecipesController : BaseController
    {
        private readonly ILogger<Recipe> _logger;
        private readonly IRecipeService _recipeService;
        private readonly IFavoriteService _favoriteService;
        private readonly ICategoryService _categoryService;

        public RecipesController(
            ILogger<Recipe> logger,
            UserManager<ApplicationUser> userManager,
            IRecipeService recipeService,
            IFavoriteService favoriteService
            ,ICategoryService categoryService) : base(userManager)
        {
            _logger = logger;
            _recipeService = recipeService;
            _favoriteService = favoriteService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string searchString, string currentFilter, List<int> categoryFilters, string currentCategoryFilters)
        {
            if (currentCategoryFilters != null)
                categoryFilters = currentCategoryFilters.Split(",").Select(int.Parse).ToList();

            ViewBag.CategoryFilters = await _categoryService.GetCategoryFilters(categoryFilters);
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategoryFilters"] = string.Join(",", categoryFilters.Select(f => f.ToString()).ToArray());

            var recipes = await _recipeService.GetRecipesList(searchString, categoryFilters);

            int pageSize = 12;

            return View(PaginatedList<RecipeDTO>.Create(recipes, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            ViewBag.IsFavorite = await _favoriteService.IsRecipeFavorite(GetCurrentUserId(), id);

            if (recipe == null)
                return NotFound();

            return View(recipe);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetCategoriesList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recipe = await _recipeService.Create(model, GetCurrentUserId());
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {

            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null)
                return NotFound();

            if (await _recipeService.RecipeBelongsToCurrentUser(recipe.Id, GetCurrentUserId()))
            {
                ViewBag.Categories = await _categoryService.GetCategoriesList();
                return View(recipe);
            }
            else
                return RedirectToAction("PermissionDenied", "Home", new { message = "You are not supposed to be on this page." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeDTO recipe)
        {
            if (id != recipe.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (!await _recipeService.RecipeExists(recipe.Id))
                    return NotFound();

                var mappedRecipe = await _recipeService.Update(recipe);

                return RedirectToAction(nameof(Details), new { id = mappedRecipe.Id });
            }
            return View(recipe);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _recipeService.GetRecipeById(id);

            if (recipe == null)
                return NotFound();

            if (await _recipeService.RecipeBelongsToCurrentUser(recipe.Id, GetCurrentUserId()))
                return View(recipe);

            else
                return RedirectToAction("PermissionDenied", "Home", new { message = "You are not supposed to be on this page." });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _recipeService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        [Route("/myrecipes")]
        [Authorize]
        public async Task<IActionResult> UserRecipes(int? pageNumber)
        {
            var userRecipes = await _recipeService.GetUserRecipes(GetCurrentUserId());
            int pageSize = 12;
            return View(PaginatedList<RecipeDTO>.Create(userRecipes, pageNumber ?? 1, pageSize));
        }
    }
}
