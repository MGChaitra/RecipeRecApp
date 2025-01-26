namespace RecipeRecWebApp.Models
{
    public static class SharedDataModel
    {
        public static List<CategoryModel> Categories = [];
        public static List<IngredientModel> Ingredients = [];
        public static List<IngredientModel> SelectedIngredients = [];

        public static event Action? OnChanged;
        public static void UpdateChanges()
        {
            OnChanged?.Invoke();
        }
    }
}
