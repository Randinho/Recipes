using Recipes.DTO;
using Recipes.ViewModels;
using System;
using System.Collections.Generic;

namespace Recipes.Interfaces
{
    public interface IShoppingListService
    {
        IEnumerable<ShoppingListItemViewModel> GetShoppingListItems(RecipeDTO recipe);
        string GenerateQRCodeString(IEnumerable<ShoppingListItemViewModel> shoppingList);
        Byte[] GenerateQRCode(string txt);
    }
}
