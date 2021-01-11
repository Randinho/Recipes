using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;


namespace Recipes.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }


        public Category()
        {
            Recipes = new Collection<Recipe>();
        }
    }
}
