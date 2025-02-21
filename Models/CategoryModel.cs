using System.Text.Json.Serialization;

namespace Models
{
    /// <summary>
    /// Represents a category of food, including the name of the food group, 
    /// whether the category is expanded, and a list of associated ingredients.
    /// </summary>
    public class CategoryModel
    {
        [JsonPropertyName("food_group")]
        public string Name { get; set; } = string.Empty;

        public bool IsExpanded { get; set; } = false;

        public List<IngredientModel> Ingredients { get; set; } = new List<IngredientModel>();
    }
}
