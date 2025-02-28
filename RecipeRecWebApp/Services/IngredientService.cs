using System.Net.Http.Json;
using Newtonsoft.Json;
using RecipeRecWebApp.Contracts;
using Models;

namespace RecipeRecWebApp.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IIngredientService> _logger;

        public IngredientService(HttpClient httpClient, ILogger<IIngredientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<IngredientModel>> GetIngredientsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching ingredients from API.");
                var ingredients = await _httpClient.GetFromJsonAsync<List<IngredientModel>>("api/AddIngredient");
                _logger.LogInformation("Ingredients fetched successfully.");
                return ingredients ?? new List<IngredientModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ingredients.");
                return new List<IngredientModel>();
            }
        }

        public async Task<bool> AddIngredientAsync(IngredientModel ingredient)
        {
            try
            {
                _logger.LogInformation("Adding ingredient to API.");
                var response = await _httpClient.PostAsJsonAsync("api/AddIngredient", ingredient);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Ingredient added successfully.");
                    return true;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to add ingredient: {ErrorMessage}", errorMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ingredient.");
                return false;
            }
        }
    }
}
