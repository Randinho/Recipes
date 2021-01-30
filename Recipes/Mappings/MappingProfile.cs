using AutoMapper;
using Recipes.DTO;
using Recipes.Models;
using Recipes.ViewModels;

namespace Recipes.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Ingredient, IngredientDTO>().ReverseMap();
            CreateMap<RecipeIngredients, RecipeIngredientsDTO>().ReverseMap();
            CreateMap<Shared, SharedDTO>().ReverseMap();
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();


            CreateMap<CreateRecipeViewModel, Recipe>();           
        }
    }
}
