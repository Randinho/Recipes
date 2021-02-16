
using Recipes.Interfaces;
using Recipes.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task Create(string ingredientName, string amount, int recipeId)
        {
            await _ingredientRepository.AddIngredient(ingredientName);

            var ingredient = await _ingredientRepository.GetIngredientByName(ingredientName);

            await _ingredientRepository.AddIngredientToRecipe(ingredient.Id, recipeId, amount);
            
        }
        public async Task Remove(int ingredientId, int recipeId) => 
            await _ingredientRepository.RemoveIngredientFromRecipe(ingredientId, recipeId);
        
    }
}
