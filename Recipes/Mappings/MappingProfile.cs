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
            CreateMap<Recipe, RecipeDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Favorite, FavoriteDTO>();
            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<RecipeIngredients, RecipeIngredientsDTO>();
            CreateMap<Shared, SharedDTO>();
            CreateMap<Notification, NotificationDTO>();

            CreateMap<RecipeDTO, Recipe>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<RecipeViewModel, Recipe>();
        }
    }
}
