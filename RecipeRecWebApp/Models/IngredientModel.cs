using System.Text.Json.Serialization;

namespace RecipeRecWebApp.Models
{
    public class IngredientModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("food_group")]
        public string categorizedName { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool Selected { get; set; } = false;
    }
}
