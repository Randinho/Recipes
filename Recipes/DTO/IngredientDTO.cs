using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.DTO
{
    public class IngredientDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public ICollection<RecipeIngredientsDTO> RecipeIngredients { get; set; }

        public IngredientDTO()
        {
            RecipeIngredients = new Collection<RecipeIngredientsDTO>();
        }
    }
}
