﻿using System.ComponentModel.DataAnnotations;

namespace Recipes.Models
{
    public class Shared
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }     
        public string ApplicationUserId { get; set; }
        [Display(Name = "Shared by")]
        public ApplicationUser ApplicationUser { get; set; }
        public bool Confirmed { get; set; }
    }
}
