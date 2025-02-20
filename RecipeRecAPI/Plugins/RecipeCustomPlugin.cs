using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Models;

namespace RecipeRecAPI.Plugins
{

    public class RecipeCustomPlugin
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RecipeCustomPlugin> _logger;   
        public RecipeCustomPlugin(IConfiguration configuration, ILogger<RecipeCustomPlugin> logger)
        {
            _configuration = configuration;
            _logger=logger;
        }

        [KernelFunction("SummarizeRecipes")]
        [Description("Generates a concise and user-friendly summary and ingredients for a recipe based on its name, instructions in the given format.")]
        public async Task<List<SummarizedRecipeModel>> SummaryRecipeAsync(string recipe, Kernel kernel)
        {

            string? promptFilePath = _configuration["Prompts:Summary"];
            if (string.IsNullOrWhiteSpace(promptFilePath))
            {
                throw new ArgumentNullException(nameof(promptFilePath));
            }
            string promptTemplate;

            try
            {
                promptTemplate = await File.ReadAllTextAsync(promptFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error reading prompt file: {ex.Message}");
                return new List<SummarizedRecipeModel>();
            }

            string prompt = promptTemplate.Replace("{recipe}", recipe);

            var result = await kernel.InvokePromptAsync<string>(prompt);
            if(result == null){
                throw new Exception("No Response");
            }
            var response=ParseSummary(result);
           
            return response;
        }

        [KernelFunction("GenerateRecipes")]
        [Description("Generates new recipe suggestions based on available ingredients.")]
        public async Task<List<RecipeModel>> GenerateRecipesAsync(string ingredients, Kernel kernel)
        {
            string? promptFilePath = _configuration["Prompts:GenerateRecipes"];
            if (string.IsNullOrWhiteSpace(promptFilePath))
            {
                throw new ArgumentNullException(nameof(promptFilePath));
            }
            string promptTemplate;

            try
            {
                promptTemplate = await File.ReadAllTextAsync(promptFilePath);
            
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error reading prompt file: {ex.Message}");
                return new List<RecipeModel>();
            }

            string prompt = promptTemplate.Replace("{ingredients}", ingredients);

            var response = await kernel.InvokePromptAsync<string>(prompt);
            if (response == null)
            {
                throw new Exception("No Response");
            }
            var recipes = ParseRecipes(response);
           
            return recipes;
        }


        private List<RecipeModel> ParseRecipes(string jsonResponse)
        {
            try
            {
                var recipes = JsonSerializer.Deserialize<List<RecipeModel>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling=JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                });

                return recipes ?? new List<RecipeModel>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing recipes: {ex.Message}");
                return new List<RecipeModel>();
            }
        }


        private List<SummarizedRecipeModel> ParseSummary(string jsonResponse)
        {
            try
            {
                var recipes = JsonSerializer.Deserialize<List<SummarizedRecipeModel>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                });

                return recipes ?? new List<SummarizedRecipeModel>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing recipes: {ex.Message}");
                return new List<SummarizedRecipeModel>();
            }
        }

    }
}
