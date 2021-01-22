using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface IIngredientService
    {
        Task Create(string ingredientName, string amount, int recipeId);
        Task Remove(int ingredientId, int recipeId);
    }
}
