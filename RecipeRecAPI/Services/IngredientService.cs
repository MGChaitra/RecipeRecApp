using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using RecipeRecAPI.Contracts;
using RecipeRecWebApp.Models;

namespace RecipeRecAPI.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly string _filepath;

        public IngredientService(IWebHostEnvironment webHostEnvironment)
        {
            _filepath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");
        }
        public List<IngredientModel> GetIngredients()
        {

            if (!File.Exists(_filepath))
                return null;

            var jsonData = File.ReadAllText(_filepath);
            return JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? new List<IngredientModel>();
        }

        public bool AddIngredient(IngredientModel newIngredient)
        {
            if (!File.Exists(_filepath))
            {
                return false;
            }
            var ingredients = GetIngredients() ?? new List<IngredientModel>();
            ingredients.Add(newIngredient);

            var updatedJson = JsonSerializer.Serialize(ingredients);
            File.WriteAllText(_filepath, updatedJson);
            return true;
        }
    }
}
