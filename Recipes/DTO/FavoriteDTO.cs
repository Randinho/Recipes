using Recipes.Models;
using System.ComponentModel.DataAnnotations;

namespace Recipes.DTO
{
    public class FavoriteDTO
    {
        public string ApplicationUserId { get; set; }
        [Display(Name = "Created by")]
        public ApplicationUserDTO ApplicationUser { get; set; }
        public int RecipeId { get; set; }
        public RecipeDTO Recipe { get; set; }
    }
}
