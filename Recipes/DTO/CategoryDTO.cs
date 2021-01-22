using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Recipes.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        [StringLength(255)]
        public string Name { get; set; }
        public ICollection<RecipeDTO> Recipes { get; set; }


        public CategoryDTO()
        {
            Recipes = new Collection<RecipeDTO>();
        }
    }
}
