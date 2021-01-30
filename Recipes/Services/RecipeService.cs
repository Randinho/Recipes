using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Interfaces.Repositories;
using Recipes.Models;
using Recipes.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRecipeRepository _recipeRepository;
   
        public RecipeService(IMapper mapper,
            IWebHostEnvironment hostEnvironment,
            IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _recipeRepository = recipeRepository;
        }
        public async Task<IEnumerable<RecipeDTO>> GetRecipesList(string searchString, List<int> categoryFilters)
        {
            var recipes = await _recipeRepository.GetRecipeList();

            if (!string.IsNullOrEmpty(searchString))
                recipes = recipes.Where(r => r.Name.Contains(searchString) || r.Description.Contains(searchString)).ToList();

            if (categoryFilters.Count != 0)
                recipes = recipes.Where(r => categoryFilters.Contains(r.CategoryId)).ToList();

            var mapped = _mapper.Map<RecipeDTO[]>(recipes);
            return mapped;
        }
        public async Task<RecipeDTO> GetRecipeById(int recipeId)
        {
            var recipe = await _recipeRepository.GetRecipeById(recipeId);
            var mapped = _mapper.Map<RecipeDTO>(recipe);
            return mapped;
        }
        public async Task<IEnumerable<RecipeDTO>> GetUserRecipes(string userId)
        {
            var recipes = await _recipeRepository.GetUserRecipes(userId);
            var mapped = _mapper.Map<RecipeDTO[]>(recipes);
            return mapped;
        }
        public async Task<RecipeDTO> Create(CreateRecipeViewModel model, string userId)
        {
            var recipe = _mapper.Map<Recipe>(model);
            recipe.Picture = UploadFile(model);
            recipe.ApplicationUserId = userId;

            await _recipeRepository.Create(recipe);

            var mapped = _mapper.Map<RecipeDTO>(recipe);
            return mapped;
        }
        public async Task<RecipeDTO> Update(RecipeDTO model)
        {
            var recipe = _mapper.Map<Recipe>(model);
            await _recipeRepository.Update(recipe);

            var mapped = _mapper.Map<RecipeDTO>(recipe);
            return mapped;
        }
        public async Task Remove(int id)
        {
            await _recipeRepository.Remove(id);
        }
        public async Task<bool> RecipeExists(int id) 
        {
            return await _recipeRepository.RecipeExists(id);
        }
        public async Task<bool> RecipeBelongsToCurrentUser(int id, string userId)
        {
            var recipe = await _recipeRepository.GetRecipeById(id);
            if (recipe.ApplicationUserId == userId)
                return true;
            return false;
        }
        private string UploadFile(CreateRecipeViewModel model)
        {
            string uniqueFileName = null;

            if (model.PictureFile != null)
            {
                string uploadFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PictureFile.FileName;
                string filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.PictureFile.CopyTo(stream);
                }
            }

            return uniqueFileName;
        }
    }
}
