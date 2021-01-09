using Recipes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.DTO
{
    public class FavoriteDTO
    {
        public string ApplicationUserId { get; set; }
        [Display(Name = "Created by")]
        public ApplicationUser ApplicationUser { get; set; }
        public int RecipeId { get; set; }
        public RecipeDTO Recipe { get; set; }
    }
}
