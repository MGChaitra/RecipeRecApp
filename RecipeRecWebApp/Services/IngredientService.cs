using Models;
using RecipeRecWebApp.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace RecipeRecWebApp.Services
{
	public class IngredientService(HttpClient httpclient, ILogger<IngredientService> logger) : IIngredientService
	{
		private readonly HttpClient _httpClient = httpclient;
		private readonly ILogger<IngredientService> logger = logger;

		// Method to get ingredients from the API
		public async Task<List<IngredientModel>> GetIngredientsAsync()
		{
			List<IngredientModel> ingredients = [];
			try
			{
				ingredients = await _httpClient.GetFromJsonAsync<List<IngredientModel>>("api/Ingredient") ?? [];
				foreach (var ingredient in ingredients)
				{
					ingredient.Selected = false;
				}
			}
			catch (Exception ex)
			{
				// Log the error or handle it appropriately
				logger.LogError($"Error: {ex.Message}");
			}
			return ingredients;
		}

		// Method to add a new ingredient via the API
		public async Task<bool> AddIngredientAsync(IngredientModel ingredient)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/Ingredient", ingredient);
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				// Log the error or handle it appropriately
				logger.LogError($"Error: {ex.Message}");
				return false;
			}
		}


		public async Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			List<RecipeModel> recipes = [];
			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/Ingredient/GetRecipes", selectedIngredients);
				response.EnsureSuccessStatusCode();

				recipes = JsonSerializer.Deserialize<List<RecipeModel>>(
					await response.Content.ReadAsStringAsync(),
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true}
					) ?? [];
			}
			catch(Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return recipes;
		}

		public void MapIngredients()
		{
			try
			{
				SharedDataModel.Categories = SharedDataModel.Ingredients
									.GroupBy(ingredient => ingredient.food_group, StringComparer.OrdinalIgnoreCase)
									.Select(group => new CategoryModel
									{
										Name = group.Key,
										Ingredients = [.. group],
										IsExpanded = false
									})
									.ToList();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

	}
}