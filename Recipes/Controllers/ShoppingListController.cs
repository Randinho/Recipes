using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Recipes.Models;
using Recipes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Drawing;
using System.IO;
using AutoMapper;

namespace Recipes.Controllers
{
    public class ShoppingListController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Ingredient> _logger;
        public ShoppingListController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            ILogger<Ingredient> logger, 
            IMapper mapper) : base(userManager, mapper)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int recipeId)
        {
            List<ShoppingListItemViewModel> shoppingList = new List<ShoppingListItemViewModel>();
            var recipe = await _context.Recipes
                .Include(x => x.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == recipeId);

            foreach(var ingredient in recipe.RecipeIngredients.ToList())
            {
                shoppingList.Add(new ShoppingListItemViewModel
                {
                    Id = ingredient.IngredientId,
                    Name = ingredient.Ingredient.Name,
                    InPossession = false
                });
            }          
            return View(shoppingList);
        }

        [HttpPost]
        public IActionResult Create(List<ShoppingListItemViewModel> shoppingList)
        {
            shoppingList = shoppingList.Where(x => x.InPossession == false).ToList();
            string txt = String.Join("\n", shoppingList.Select(s => s.Name).ToArray());
            TempData["QRCodeText"] = txt;
            return RedirectToAction(nameof(ShoppingListCode));
        }

        
        public IActionResult ShoppingListCode()
        {
            var txt = TempData["QRCodeText"] as string;

            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(txt,
                QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return View(BitmapToBytes(qrCodeImage));
        }

        [NonAction]
        private static Byte[] BitmapToBytes(Bitmap image)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
