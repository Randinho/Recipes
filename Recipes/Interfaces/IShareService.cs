using Recipes.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface IShareService
    {
        Task<IEnumerable<SharedDTO>> GetSharedRecipesList(string userId);
        Task<bool> ShareRecipe(int id, string email);
    }
}
