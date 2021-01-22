using Recipes.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Recipes.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]

        public string Name { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]
        [Display(Name = "Is private?")]

        public bool IsPrivate { get; set; }
        public string Picture { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public CategoryDTO Category { get; set; }
        public int CategoryId { get; set; }

        public ICollection<FavoriteDTO> Favorites { get; set; }
        public ICollection<RecipeIngredientsDTO> RecipeIngredients { get; set; }
        public ICollection<SharedDTO> Shared { get; set; }

        public RecipeDTO()
        {
            Favorites = new Collection<FavoriteDTO>();
            RecipeIngredients = new Collection<RecipeIngredientsDTO>();
            Shared = new Collection<SharedDTO>();
        }
    }
}
