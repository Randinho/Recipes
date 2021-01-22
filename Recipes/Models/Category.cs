using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Recipes.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }


        public Category()
        {
            Recipes = new Collection<Recipe>();
        }
    }
}
