using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces.Repositories
{
    public interface IIngredientRepository
    {
        Task AddIngredient(string name);
        Task<Ingredient> GetIngredientByName(string name);
        Task AddIngredientToRecipe(int ingredientId, int recipeId, string amount);
        Task RemoveIngredientFromRecipe(int ingredientId, int recipeId);
    }
}
