using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;


namespace Recipes.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Picture { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<RecipeIngredients> RecipeIngredients { get; set; }
        public ICollection<Shared> Shared { get; set; }

        public Recipe()
        {
            Favorites = new Collection<Favorite>();
            RecipeIngredients = new Collection<RecipeIngredients>();
            Shared = new Collection<Shared>();
        }
    }
}
