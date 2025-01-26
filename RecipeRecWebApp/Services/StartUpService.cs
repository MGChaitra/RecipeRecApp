
using System.Text.Json;
using System.Text.Json.Serialization;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Models;

namespace RecipeRecWebApp.Services
{
    public class StartUpService(ILogger<StartUpService> logger, HttpClient client) : IStartUpService
    {
        private readonly ILogger<StartUpService> logger = logger;
        private readonly HttpClient client = client;

        public async Task InitializeIngredientsAsync()
        {
            try
            {
                logger.LogInformation("Loading Data...");
                var jsonData = await client.GetStringAsync("Data/FoodDB.json");
                SharedDataModel.Ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? [];
                var DuplicateCategories = JsonSerializer.Deserialize<List<CategoryModel>>(jsonData, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }) ?? [];
                SharedDataModel.Categories = DuplicateCategories.GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase).Select(group => group.First()).ToList();

                foreach (var category in SharedDataModel.Categories)
                {
                    category.IsExpanded = false;
                    foreach (var ingredient in SharedDataModel.Ingredients)
                    {
                        if (category.Name == ingredient.categorizedName)
                        {
                            category.Ingredients.Add(ingredient);
                            ingredient.Visible = false;
                            ingredient.Selected = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }

        }
    }
}
