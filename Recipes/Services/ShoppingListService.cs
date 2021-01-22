using Recipes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using AutoMapper;
using Recipes.ViewModels;
using Recipes.DTO;
using QRCoder;
using System.Drawing;
using System.IO;

namespace Recipes.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShoppingListService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public IEnumerable<ShoppingListItemViewModel> GetShoppingListItems(RecipeDTO recipe)
        {
            List<ShoppingListItemViewModel> shoppingList = new List<ShoppingListItemViewModel>();

            foreach(var item in recipe.RecipeIngredients.ToList())
            {
                shoppingList.Add(new ShoppingListItemViewModel
                {
                    Id = item.IngredientId,
                    Name = item.Ingredient.Name,
                    InPossession = false
                });
            }
            return shoppingList;
        }
        public string GenerateQRCodeString(IEnumerable<ShoppingListItemViewModel> shoppingList)
        {
            shoppingList = shoppingList.Where(x => x.InPossession == false).ToList();
            string txt = string.Join("\n", shoppingList.Select(s => s.Name).ToArray());
            return txt;
        }
        public Byte[] GenerateQRCode(string txt)
        {
            Bitmap image = GenerateImage(txt);
            using(MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private Bitmap GenerateImage(string txt)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(txt,
                QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }
    }
}
