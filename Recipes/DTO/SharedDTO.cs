using Recipes.Models;
using System.ComponentModel.DataAnnotations;

namespace Recipes.DTO
{
    public class SharedDTO
    {
        public int RecipeId { get; set; }
        public RecipeDTO Recipe { get; set; }
        public string ApplicationUserId { get; set; }
        [Display(Name = "Shared by")]
        public ApplicationUserDTO ApplicationUser { get; set; }
        public bool Confirmed { get; set; }
    }
}
