using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using RecipeRecAPI.Contracts;
using Models;

namespace RecipeRecAPI.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly string _filepath;
        private readonly ILogger<IIngredientService> _logger;

        public IngredientService(IWebHostEnvironment webHostEnvironment, ILogger<IIngredientService> logger)
        {
            _logger = logger;
            _filepath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");
        }

        public List<IngredientModel> GetIngredients()
        {
            try
            {
                if (!File.Exists(_filepath))
                {
                    _logger.LogWarning("File not found: {FilePath}", _filepath);
                    return new List<IngredientModel>();
                }

                var jsonData = File.ReadAllText(_filepath);
                _logger.LogInformation("File read successfully: {FilePath}", _filepath);
                return JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? new List<IngredientModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading ingredients from file: {FilePath}", _filepath);
                return new List<IngredientModel>();
            }
        }

        public bool AddIngredient(IngredientModel newIngredient)
        {
            try
            {
                if (!File.Exists(_filepath))
                {
                    _logger.LogWarning("File not found: {FilePath}", _filepath);
                    return false;
                }

                var ingredients = GetIngredients() ?? new List<IngredientModel>();
                ingredients.Add(newIngredient);

                var updatedJson = JsonSerializer.Serialize(ingredients);
                File.WriteAllText(_filepath, updatedJson);
                _logger.LogInformation("Ingredient added successfully: {IngredientName}", newIngredient.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ingredient: {IngredientName}", newIngredient.Name);
                return false;
            }
        }
    }
}
