using System.Net.Http.Json;
using Newtonsoft.Json;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Models;

namespace RecipeRecWebApp.Services
{
    public class IngredientService: IIngredientService
    {
        private readonly HttpClient _httpClient;

        public IngredientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to get ingredients from the API
        public async Task<List<IngredientModel>> GetIngredientsAsync()
        {
            try
            {
                var ingredients = await _httpClient.GetFromJsonAsync<List<IngredientModel>>("api/AddIngrdient");
                return ingredients ?? new List<IngredientModel>();
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Console.WriteLine($"Error fetching ingredients: {ex.Message}");
                return new List<IngredientModel>();
            }
        }

        // Method to add a new ingredient via the API
        public async Task<bool> AddIngredientAsync(IngredientModel ingredient)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/AddIngrdient", ingredient);
          

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Console.WriteLine($"Error adding ingredient: {ex.Message}");
                return false;
            }
        }
    }
}
