using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<RecipeIngredients> RecipeIngredients { get; set; }

        public Ingredient()
        {
            RecipeIngredients = new Collection<RecipeIngredients>();
        }
    }
}
