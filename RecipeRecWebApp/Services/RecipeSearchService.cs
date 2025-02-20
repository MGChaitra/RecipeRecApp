using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Logging;
using Models;
using RecipeRecWebApp.Contracts;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace RecipeRecWebApp.Services
{
    public class RecipeSearchService : IRecipeSearchService
    {
       
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public RecipeSearchService(ILogger<RecipeSearchService> logger, HttpClient httpClient)
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

        public async Task<SummarizedRecipeModel> SummarizeRecipeAsync(RecipeModel recipe)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/RecipeCustom/summarize", new List<RecipeModel> { recipe });
                if (response.IsSuccessStatusCode)
                {
                    var summaryList = await response.Content.ReadFromJsonAsync<List<SummarizedRecipeModel>>();
                    foreach (var sum in summaryList)
                    {
                      return sum;
                    }
                 
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching summary: {ex.Message}");
                return null;
            }
        }

        public async Task StoreRecipeAsync(List<RecipeModel> recipes)
        {
            if (recipes == null || recipes.Count == 0)
            {
                _logger.LogWarning("No recipes to store.");
                return;
            }

            try
            {

                var response = await _httpClient.PostAsJsonAsync("api/RecipeSearch/upload-recipes", recipes);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully stored {0} recipes in Azure Search index", recipes.Count);

                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to store recipes in Azure Search. Status: {0}, Error: {1}", response.StatusCode, errorMessage);
                }

            }
            catch (RequestFailedException ex)
            {
                _logger.LogError("Failed to store recipes in Azure Search. Error: {0}", ex.Message);
            }
        }
    }
}