using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Recipes.ViewModels
{
    public class CreateRecipeViewModel
    {
        //public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Is private?")]
        public bool IsPrivate { get; set; }

        [Required]
        [Display(Name = "Picture")]
        public IFormFile PictureFile { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
