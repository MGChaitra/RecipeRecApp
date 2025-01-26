using System.Text.Json.Serialization;

namespace RecipeRecWebApp.Models
{
    public class CategoryModel
    {

        [JsonPropertyName("food_group")]
        public string Name { get; set; } = string.Empty;
        public bool IsExpanded { get; set; } = false;
        public List<IngredientModel> Ingredients { get; set; } = [];
    }
}
