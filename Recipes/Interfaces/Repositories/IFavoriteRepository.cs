using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces.Repositories
{
    public interface IFavoriteRepository
    {
        Task<Favorite> GetFavoriteRecipe(string userId, int recipeId);
    }
}
