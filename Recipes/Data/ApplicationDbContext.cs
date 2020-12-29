using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; } 
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
        public DbSet<Shared> Shared { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Favorite>().HasKey(f => new { f.ApplicationUserId, f.RecipeId });
            builder.Entity<RecipeIngredients>().HasKey(ri => new { ri.RecipeId, ri.IngredientId });
            builder.Entity<Shared>().HasKey(s => new { s.RecipeId, s.ApplicationUserId });

        }
    }
}
