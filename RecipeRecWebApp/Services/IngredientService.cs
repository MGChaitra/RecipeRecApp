using System.Net.Http.Json;
using Newtonsoft.Json;
using RecipeRecWebApp.Contracts;
using Models;

namespace RecipeRecWebApp.Services
{
    public class IngredientService: IIngredientService
    {
        private readonly HttpClient _httpClient;

        public IngredientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       
        public async Task<List<IngredientModel>> GetIngredientsAsync()
        {
            try
            {
                var ingredients = await _httpClient.GetFromJsonAsync<List<IngredientModel>>("api/AddIngredient");
                return ingredients ?? new List<IngredientModel>();
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Error fetching ingredients: {ex.Message}");
                return new List<IngredientModel>();
            }
        }

        public async Task<bool> AddIngredientAsync(IngredientModel ingredient)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/AddIngredient", ingredient);
          

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error adding ingredient: {ex.Message}");
                return false;
            }
        }
    }
}
