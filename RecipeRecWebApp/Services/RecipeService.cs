using System.Net.Http.Json;
using Models;
using System.Text.Json;
using RecipeRecWebApp.Contracts;

namespace RecipeRecWebApp.Services
{
	public class RecipeService(HttpClient httpclient, ILogger<RecipeService> logger) : IRecipeService
	{
		private readonly HttpClient _httpClient = httpclient;
		private readonly ILogger<RecipeService> logger = logger;

		public async Task<List<string>> CustomInstructions(RecipeModel recipe)
		{
			List<string> instructions = [];
			try
			{
				var res = await _httpClient.PostAsJsonAsync("api/Recipes/CustomizeInstructions", recipe);
				res.EnsureSuccessStatusCode();

				instructions = JsonSerializer.Deserialize<List<string>>(
					await res.Content.ReadAsStringAsync(),
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
					) ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return instructions;
		}

		public async Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			List<RecipeModel> recipes = [];
			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/Recipes/GetRecipes", selectedIngredients);
				response.EnsureSuccessStatusCode();

				recipes = JsonSerializer.Deserialize<List<RecipeModel>>(
					await response.Content.ReadAsStringAsync(),
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
					) ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return recipes;
		}
	}
}
