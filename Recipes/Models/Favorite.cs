using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Models
{
    public class Favorite
    {
        public string ApplicationUserId { get; set; }
        [Display(Name = "Created by")]
        public ApplicationUser ApplicationUser { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
