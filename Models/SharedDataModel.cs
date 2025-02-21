namespace Models
{
    /// <summary>
    /// Represents shared data for the application, including lists of categories, ingredients, 
    /// selected ingredients, and an event to notify when changes occur.
    /// </summary>
    public class SharedDataModel
    {
        public static List<CategoryModel> Categories = new List<CategoryModel>();
        public static List<IngredientModel> Ingredients = new List<IngredientModel>();
        public static List<IngredientModel> SelectedIngredients = new List<IngredientModel>();
        public static event Action? OnChanged;

        public static void UpdateChanges()
        {
            OnChanged?.Invoke();
        }
    }
}
