namespace Models

{
    public class SharedDataModel
    {
        public static List<CategoryModel> Categories = [];
        public static List<IngredientModel> Ingredients = [];
        public static List<IngredientModel> SelectedIngredients = [];
        //public static List<RecipeModel> FavoriteRecipes= new List<RecipeModel>();
        public static event Action? OnChanged;
        public static void UpdateChanges()
        {
            OnChanged?.Invoke();
        }
    }
}
