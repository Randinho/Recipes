using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.DTO;
using Recipes.ViewModels;

namespace Recipes.Interfaces
{
    public interface IShoppingListService
    {
       IEnumerable<ShoppingListItemViewModel> GetShoppingListItems(RecipeDTO recipe);
        string GenerateQRCodeString(IEnumerable<ShoppingListItemViewModel> shoppingList);
        Byte[] GenerateQRCode(string txt);
    }
}
