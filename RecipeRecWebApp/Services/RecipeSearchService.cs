using Microsoft.Extensions.Logging;
using Models;
using RecipeRecWebApp.Contracts;
using System.Net.Http;
using System.Net.Http.Json;


namespace RecipeRecWebApp.Services
{
    public class RecipeSearchService : IRecipeSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RecipeSearchService> _logger;

        public RecipeSearchService(HttpClient httpClient, ILogger<RecipeSearchService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<RecipeModel>> SearchRecipesAsync(List<string> ingredients)
        {
            string query = string.Join(", ", ingredients);
            try
            {
                var recipes = await _httpClient.GetFromJsonAsync<List<RecipeModel>>($"api/RecipeSearch/search?query={query}") ?? new List<RecipeModel>();
                return recipes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching recipes: {ex.Message}");
                return new List<RecipeModel>();
            }
        }

        public async Task<List<RecipeModel>> GenerateRecipesAsync(List<string> ingredients)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/RecipeCustom/generate", ingredients);
                if (response.IsSuccessStatusCode)
                {
                    var recipes = await response.Content.ReadFromJsonAsync<List<RecipeModel>>() ?? new List<RecipeModel>();
                    return recipes;
                }
                return new List<RecipeModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating recipes: {ex.Message}");
                return new List<RecipeModel>();
            }
        }

        public async Task<string?> SummarizeRecipeAsync(RecipeModel recipe)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/RecipeCustom/summarize", new List<RecipeModel> { recipe });
                if (response.IsSuccessStatusCode)
                {
                    var summaryList = await response.Content.ReadFromJsonAsync<List<SummarizedRecipeModel>>();
                    return summaryList?.FirstOrDefault()?.Summary;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching summary: {ex.Message}");
                return null;
            }
        }
    }
}

