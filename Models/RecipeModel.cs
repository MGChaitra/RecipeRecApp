using System.Text.Json.Serialization;

namespace Models
{
	public class RecipeModel
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("Name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("Description")]
		public string Description { get; set; } = string.Empty;

		[JsonPropertyName("Ingredients")]
		public List<IngredientModel> Ingredients { get; set; } = [];

		[JsonPropertyName("Instructions")]
		public List<string> Instructions { get; set; } = [];

		[JsonPropertyName("IsVeg")]
		public bool IsVeg { get; set; } = true;

		public bool IsFav = false;
	}
}
