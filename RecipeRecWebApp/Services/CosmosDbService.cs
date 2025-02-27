using System.Net.Http.Json;
using Microsoft.Azure.Cosmos;
using Models;
using RecipeRecWebApp.Contracts;

namespace RecipeRecWebApp.Services
{
    public class CosmosDbService: ICosmosDbService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CosmosDbService> _logger;

        public CosmosDbService(HttpClient httpClient, ILogger<CosmosDbService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<FavoriteRecipeModel>> GetAllFavRecipe()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<FavoriteRecipeModel>>("api/Cosmos") ?? new List<FavoriteRecipeModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching recipes: {ex.Message}");
                return new List<FavoriteRecipeModel>();
            }
        }

        public async Task<bool> AddToFavorites(FavoriteRecipeModel recipe)
        {
            try
            {
                var allFav = await _httpClient.GetFromJsonAsync<List<FavoriteRecipeModel>>("api/Cosmos") ?? new List<FavoriteRecipeModel>();

                if (allFav.Any(fav => fav.recipe_name == recipe.recipe_name))
                {
                    _logger.LogWarning("Favorite Recipe already exists");
                    return false;
                }

                var response = await _httpClient.PostAsJsonAsync($"/api/Cosmos", recipe);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding recipe: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> RemoveFromFv(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Cosmos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting recipe: {ex.Message}");
                
            }
            return false;
        }
    }
}
