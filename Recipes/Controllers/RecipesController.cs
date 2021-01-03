﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using Recipes.ViewModels;

namespace Recipes.Controllers
{
    
    public class RecipesController : BaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger<Recipe> logger;      

        public RecipesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<Recipe> logger, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this.context = context;
            hostEnvironment = webHostEnvironment;
            this.logger = logger;
           
        }

        public List<CategoryFilterViewModel> GetCategoryFilters(List<int> checkedFilters)
        {
            var categories = context.Categories.ToList();
            var filters = new List<CategoryFilterViewModel>();
            foreach(var item in categories)
            {
                var filter = new CategoryFilterViewModel
                {
                    Name = item.Name,
                    Id = item.Id
                };
                if (checkedFilters.Contains(item.Id)){
                    filter.IsChecked = true;
                }
                else
                {
                    filter.IsChecked = false;
                }
                filters.Add(filter);           
            }
            return filters;

        }
            
        public IActionResult Index(int? pageNumber, string searchString, string currentFilter, List<int> categoryFilters, string currentCategoryFilters)
        {   
            if(currentCategoryFilters != null)
            {
                categoryFilters = currentCategoryFilters.Split(",").Select(int.Parse).ToList();
            }
            ViewBag.CategoryFilters = GetCategoryFilters(categoryFilters);
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategoryFilters"] = string.Join(",", categoryFilters.Select(f => f.ToString()).ToArray());       

            var recipes = context.Recipes
                .Include(x => x.Category)
                .Include(x => x.ApplicationUser)
                .Where(x => x.IsPrivate == false).ToList();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(r => r.Name.Contains(searchString) || r.Description.Contains(searchString)).ToList();
            }
            if(categoryFilters.Count != 0)
            recipes = recipes.Where(r => categoryFilters.Contains(r.CategoryId)).ToList();

            int pageSize = 12;

            return View(PaginatedList<Recipe>.Create(recipes, pageNumber ?? 1, pageSize));
        }
      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await context.Recipes
                .Include(m => m.Category)
                .Include(m => m.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            var isFavorite = await context.Favorites.FirstOrDefaultAsync(x => x.ApplicationUserId == GetCurrentUserId() && x.RecipeId == recipe.Id);
          
            if (isFavorite != null)
                ViewBag.IsFavorite = true;
            if (recipe == null)
            {
                return NotFound();
            }
            ViewBag.Ingredients = await context.RecipeIngredients.Include(x => x.Ingredient).Where(x => x.RecipeId == id).ToListAsync();
            return View(recipe);
        }
      
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await context.Categories.ToListAsync();
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model);
                Recipe recipe = new Recipe
                {
                    Name = model.Name,
                    Description = model.Description,
                    IsPrivate = model.IsPrivate,
                    Picture = uniqueFileName,
                    ApplicationUserId = GetCurrentUserId(),
                    CategoryId = model.CategoryId
                };
                context.Add(recipe);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadFile(RecipeViewModel model)
        {
            string uniqueFileName = null;
            
            if(model.PictureFile != null)
            {
                string uploadFolderPath = Path.Combine(hostEnvironment.WebRootPath, "images");               
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PictureFile.FileName;
                string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
   
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.PictureFile.CopyTo(stream);
                    
                }
            }
            logger.LogInformation("unique: " + uniqueFileName);
            return uniqueFileName;
        }
      
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await context.Recipes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            if(RecipeBelongsToCurrentUser(recipe.Id))
            {
                ViewBag.Ingredients = await context.RecipeIngredients.Include(x => x.Ingredient).Where(x => x.RecipeId == id).ToListAsync();
                ViewBag.Categories = await context.Categories.ToListAsync();
                return View(recipe);
            }
            else
            {
                
                return RedirectToAction("PermissionDenied", "Home", new { message = "You are not supposed to be on this page." });
            }
            
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     var Recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id == id);                 
                        Recipe.Name = recipe.Name;
                        Recipe.Description = recipe.Description;
                        Recipe.IsPrivate = recipe.IsPrivate;
                        Recipe.CategoryId = recipe.CategoryId;
                    
                    context.Update(Recipe);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserRecipes));
            }
            return View(recipe);
        }
      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            if (RecipeBelongsToCurrentUser(recipe.Id)){
                return View(recipe);
            }
            else
            {
                return RedirectToAction("PermissionDenied", "Home", new { message = "You are not supposed to be on this page." });
            }        
        }
    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await context.Recipes.FindAsync(id);
            context.Recipes.Remove(recipe);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return context.Recipes.Any(e => e.Id == id);
        }

        [Route("/myrecipes")]
        [Authorize]
        public async Task<IActionResult> UserRecipes()
        {
            return View(await context.Recipes.Include(x => x.Category).Where(x => x.ApplicationUserId == GetCurrentUserId()).ToListAsync());
        }

        public bool RecipeBelongsToCurrentUser(int recipeId)
        {
            var recipe = context.Recipes.FirstOrDefault(x => x.Id == recipeId);

            if (recipe.ApplicationUserId == GetCurrentUserId())
                return true;
            return false;
        }
    }
}
