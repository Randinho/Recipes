using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.Interfaces;
using Recipes.Models;
using Recipes.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class ShoppingListController : BaseController
    {
        private readonly ILogger<Ingredient> _logger;
        private readonly IRecipeService _recipeService;
        private readonly IShoppingListService _shoppingListService;
        public ShoppingListController(UserManager<ApplicationUser> userManager,
            ILogger<Ingredient> logger,
            IRecipeService recipeService,
            IShoppingListService shoppingListService) : base(userManager)
        {
            _logger = logger;
            _recipeService = recipeService;
            _shoppingListService = shoppingListService;
        }

        public async Task<IActionResult> Index(int recipeId)
        {
            var recipe = await _recipeService.GetRecipeById(recipeId);

            var shoppingList = _shoppingListService.GetShoppingListItems(recipe);
            return View(shoppingList);
        }

        [HttpPost]
        public IActionResult Create(IEnumerable<ShoppingListItemViewModel> shoppingList)
        {
            TempData["QRCodeText"] = _shoppingListService.GenerateQRCodeString(shoppingList);
            return RedirectToAction(nameof(ShoppingListCode));
        }


        public IActionResult ShoppingListCode()
        {
            var txt = TempData["QRCodeText"] as string;

            return View(_shoppingListService.GenerateQRCode(txt));
        }
    }
}
