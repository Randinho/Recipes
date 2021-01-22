using Recipes.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface IShareService
    {
        Task<IEnumerable<SharedDTO>> GetSharedRecipesList(string userId);
        Task<bool> ShareRecipe(int id, string email);
    }
}
