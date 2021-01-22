using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Recipes.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeIngredients> RecipeIngredients { get; set; }

        public Ingredient()
        {
            RecipeIngredients = new Collection<RecipeIngredients>();
        }
    }
}
