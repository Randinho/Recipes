using Recipes.DTO;
using Recipes.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDTO>> GetRecipesList(string searchString, List<int> categoryFilters);
        Task<RecipeDTO> GetRecipeById(int recipeId);
        Task<IEnumerable<RecipeDTO>> GetUserRecipes(string userId);
        Task<RecipeDTO> Create(CreateRecipeViewModel model, string userId);
        Task<RecipeDTO> Update(RecipeDTO model);
        Task Remove(int id);  
        Task<bool> RecipeExists(int id);
        Task<bool> RecipeBelongsToCurrentUser(int id, string userId);
    }
}
