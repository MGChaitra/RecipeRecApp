using System.Text.Json;
using Models;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Services
{
	public class IngredientService(IWebHostEnvironment webHostEnvironment, ILogger<IngredientService> logger) : IIngredientService
	{
		private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
		private readonly ILogger<IngredientService> logger = logger;

		public void AddIngredient(IngredientModel newIngredient)
		{
			logger.LogInformation("Adding Ingredient");
			try
			{
				var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");

				var jsonData = File.ReadAllText(filePath);
				var ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? [];
				var item = ingredients.FirstOrDefault(items => String.Equals(items.Name, newIngredient.Name, StringComparison.OrdinalIgnoreCase));
				if (item == null) ingredients.Add(newIngredient);


				var updatedJson = JsonSerializer.Serialize(ingredients);
				File.WriteAllText(filePath, updatedJson);
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		public List<IngredientModel> GetIngredients()
		{
			List<IngredientModel> ingredients = [];
			try
			{
				var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");
				if (!File.Exists(filePath)) return [];

				var jsonData = File.ReadAllText(filePath);
				ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

			return ingredients;
		}
	}
}
