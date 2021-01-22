using Recipes.DTO;
using Recipes.Models;
using Recipes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDTO>> GetRecipesList(string searchString, List<int> categoryFilters);
        Task<RecipeDTO> GetRecipeById(int recipeId);
        Task<IEnumerable<RecipeDTO>> GetUserRecipes(string userId);
        Task<RecipeDTO> Create(RecipeViewModel model, string userId);
        Task<RecipeDTO> Update(RecipeDTO model);
        Task Remove(int id);
        Task<bool> CheckIfRecipeIsFavorite(string userId, int recipeId);
        Task<IEnumerable<CategoryFilterViewModel>> GetCategoryFilters(IEnumerable<int> checkedFilters);
        Task<IEnumerable<CategoryDTO>> GetCategoriesList();
        Task<bool> RecipeExists(int id);
        Task<bool> RecipeBelongsToCurrentUser(int id, string userId); 
    }
}
