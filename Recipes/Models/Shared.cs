namespace Recipes.Models
{
    public class Shared
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool Confirmed { get; set; }
    }
}
