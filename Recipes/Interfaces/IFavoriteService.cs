using Recipes.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Recipes.Interfaces
{

    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDTO>> GetFavoriteList(string userId);
        Task Add(int recipeId, string userId);
        Task Remove(int recipeId, string userId);
        Task<bool> IsRecipeFavorite(string userId, int recipeId);
    }
}
