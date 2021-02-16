using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;

  namespace Recipes.Interfaces.Repositories
{
    public interface IShareRepository
    {
        Task<IEnumerable<Shared>> GetSharedRecipesList(string userId);
        Task<bool> IsAlreadyShared(int recipeId, string userId);
        Task AddShared(string userId, int recipeId);
        
    }
}
