﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FavoriteService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(int recipeId, string userId)
        {
            await _context.Favorites.AddAsync(new Favorite
            {
                ApplicationUserId = userId,
                RecipeId = recipeId
            });
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<FavoriteDTO>> GetFavoriteList(string userId)
        {
            var favoriteRecipes = await _context.Favorites
                .Include(x => x.Recipe.ApplicationUser)
                .Include(x => x.Recipe.Category)
                .Where(x => x.ApplicationUserId == userId).ToListAsync();
            var mappedFavorites = _mapper.Map<FavoriteDTO[]>(favoriteRecipes);
            return mappedFavorites;
        }
        public async Task Remove(int recipeId, string userId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
