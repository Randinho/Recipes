using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Interfaces;
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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment; 
        public RecipeService(ApplicationDbContext dbContext, 
            IMapper mapper,
            IWebHostEnvironment hostEnvironment)
        {
            _context = dbContext;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IEnumerable<RecipeDTO>> GetRecipesList(string searchString, List<int> categoryFilters)
        {
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.ApplicationUser)
                .Where(r => r.IsPrivate == false).ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
                recipes = recipes.Where(r => r.Name.Contains(searchString) || r.Description.Contains(searchString)).ToList();

            if (categoryFilters.Count != 0)
                recipes = recipes.Where(r => categoryFilters.Contains(r.CategoryId)).ToList();

            var mapped = _mapper.Map<RecipeDTO[]>(recipes);
            return mapped;
        }
        public async Task<RecipeDTO> GetRecipeById(int recipeId)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.ApplicationUser)
                .Include(r => r.RecipeIngredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
            var mapped = _mapper.Map<RecipeDTO>(recipe);
            return mapped;
        }
        public async Task<IEnumerable<RecipeDTO>> GetUserRecipes(string userId)
        {
            var recipes = await _context.Recipes
                .Include(r => r.Category)
                .Where(r => r.ApplicationUserId == userId).ToListAsync();
            var mapped = _mapper.Map<RecipeDTO[]>(recipes);
            return mapped;
        }
        public async Task<RecipeDTO> Create(RecipeViewModel model, string userId)
        {
            var recipe = _mapper.Map<Recipe>(model);
            recipe.Picture = UploadFile(model);
            recipe.ApplicationUserId = userId;

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            var mapped = _mapper.Map<RecipeDTO>(recipe);
            return mapped;
        } 
        public async Task<RecipeDTO> Update(RecipeDTO model)
        {
            var recipe = _mapper.Map<Recipe>(model);
            _context.Update(recipe);
            await _context.SaveChangesAsync();

            var mapped = _mapper.Map<RecipeDTO>(recipe);
            return mapped;
        }
        public async Task Remove(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            _context.Remove(recipe);
            await _context.SaveChangesAsync();
        }    
        public async Task<IEnumerable<CategoryFilterViewModel>> GetCategoryFilters(IEnumerable<int> checkedFilters)
        {
            var categories = await _context.Categories.ToListAsync();
            var filters = new List<CategoryFilterViewModel>();
            foreach (var item in categories)
            {
                var filter = new CategoryFilterViewModel
                {
                    Name = item.Name,
                    Id = item.Id
                };
                filter.IsChecked = checkedFilters.Contains(item.Id);
                filters.Add(filter);
            }
            return filters;
        }
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesList()
        {
            var categories = await _context.Categories.ToListAsync();
            var mapped = _mapper.Map<CategoryDTO[]>(categories);
            return mapped;
        }
        public async Task<bool> CheckIfRecipeIsFavorite(string userId, int recipeId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);
            if (favorite != null)
                return true;
            return false;
        }
        public async Task<bool> RecipeExists(int id) => await _context.Recipes.AnyAsync(r => r.Id == id);
        public async Task<bool> RecipeBelongsToCurrentUser(int id, string userId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            if (recipe.ApplicationUserId == userId)
                return true;
            return false;
        }
        private string UploadFile(RecipeViewModel model)
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
