using System.Net.Http.Json;
using Models;
using RecipeRecWebApp.Contracts;

namespace RecipeRecWebApp.Services
{
	public class FavoritesService(HttpClient httpClient, ILogger<FavoritesService> logger) : IFavoritesService
	{
		private readonly HttpClient httpClient = httpClient;
		private readonly ILogger<FavoritesService> logger = logger;

		public async Task<string> SaveFavorite(RecipeModel recipe)
		{
			var message = "";
			try
			{
				var response = await httpClient.PostAsJsonAsync("api/Favorites", recipe);
				message = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				message = "Http BadRequest";
				logger.LogError($"Error: {ex.Message}");
			}

			return message;
		}

		public async Task<string> RemoveFavorite(RecipeModel recipe)
		{
			var message = "";
			try
			{
				var response = await httpClient.DeleteAsync($"api/Favorites/{recipe.Id}");
				message = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				message = "Http BadRequest";
				logger.LogError($"Error: {ex.Message}");
			}
			return message;
		}

		public async Task GetAllFavorites()
		{
			try
			{
				SharedDataModel.FavoriteRecipes = await httpClient.GetFromJsonAsync<List<RecipeModel>>($"api/Favorites") ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		public async Task<string> UpdateFavorites(RecipeModel recipe)
		{
			string message = "";
			try
			{
				var response = await httpClient.PatchAsJsonAsync("api/Favorites", recipe);
				message = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				message = "Http BadRequest";
				logger.LogError($"Error: {ex.Message}");
			}
			return message;
		}
	}
}
