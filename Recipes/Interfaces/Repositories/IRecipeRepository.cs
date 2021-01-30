using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetRecipeList();
        Task<Recipe> GetRecipeById(int id);
        Task<IEnumerable<Recipe>> GetUserRecipes(string userId);
        Task Create(Recipe recipe);
        Task Update(Recipe recipe);
        Task Remove(int id);
        Task<bool> RecipeExists(int id);
    }
}
