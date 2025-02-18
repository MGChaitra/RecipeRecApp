namespace Models
{
	public class SharedDataModel
	{
		public static List<CategoryModel> Categories = [];
		public static List<IngredientModel> Ingredients = [];
		public static List<IngredientModel> SelectedIngredients = [];
		public static List<RecipeModel> Recipes = [];
		public static List<RecipeModel> FavoriteRecipes = [];

		public static bool selectIngredients = true;
		public static bool displayRecipe = true;

		public static event Action? OnChanged;
		public static void UpdateChanges()
		{
			OnChanged?.Invoke();
		}
	}
}
