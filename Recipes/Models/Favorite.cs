using System.ComponentModel.DataAnnotations;


namespace Recipes.Models
{
    public class Favorite
    {
        public string ApplicationUserId { get; set; }   
        public ApplicationUser ApplicationUser { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
