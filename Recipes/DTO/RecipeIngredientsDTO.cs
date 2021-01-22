namespace Recipes.DTO
{
    public class RecipeIngredientsDTO
    {
        public int RecipeId { get; set; }
        public RecipeDTO Recipe { get; set; }
        public int IngredientId { get; set; }
        public IngredientDTO Ingredient { get; set; }
        public string Amount { get; set; }
    }
}
