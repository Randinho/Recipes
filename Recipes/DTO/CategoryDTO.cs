using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Name { get; set; }
        public ICollection<RecipeDTO> Recipes { get; set; }


        public CategoryDTO()
        {
            Recipes = new Collection<RecipeDTO>();
        }
    }
}
