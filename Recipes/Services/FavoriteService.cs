using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Interfaces.Repositories;
using Recipes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMapper _mapper;
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IMapper mapper,
            IFavoriteRepository favoriteRepository)
        {
            _mapper = mapper;
            _favoriteRepository = favoriteRepository;
        }

        public async Task Add(int recipeId, string userId)
        {
            await _favoriteRepository.AddToFavorites(userId, recipeId);
        }
        public async Task<IEnumerable<FavoriteDTO>> GetFavoriteList(string userId)
        {
            var favoriteRecipes = await _favoriteRepository.GetFavoriteList(userId);
            var mappedFavorites = _mapper.Map<FavoriteDTO[]>(favoriteRecipes);
            return mappedFavorites;
        }
        public async Task Remove(int recipeId, string userId)
        {
            var favorite = await _favoriteRepository.GetFavoriteRecipe(userId, recipeId);
            if (favorite != null)
            {
                await _favoriteRepository.Remove(favorite);
            }
        }
        public async Task<bool> IsRecipeFavorite(string userId, int recipeId)
        {
            var favorite = await _favoriteRepository.GetFavoriteRecipe(userId, recipeId);
            if (favorite != null)
                return true;
            return false;
        }
    }
}
