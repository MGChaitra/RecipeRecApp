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

        public RecipeCustomPlugin(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [KernelFunction("SummarizeRecipes")]
        [Description("Generates a concise and user-friendly summary for a recipe based on its name, ingredients, and instructions.")]
        public async Task<string> SummaryRecipeAsync(string recipe, Kernel kernel)
        {
            string prompt = $@"
You are a professional chef and food writer. Your task is to create a **short, user-friendly recipe summary** for the given recipe.

### Recipe Details:
{recipe}

### Instructions:
- Summarize the key highlights of this recipe in **more detail**.
- Include the **main ingredients** without listing every detail.
- Also make use of **measurments** for the ingredients needed for the particular recipe.
- Focus on the **cooking process** and what makes this recipe unique.
- Keep the tone **engaging and food-friendly**.

### Example Summary:
For a classic **Spaghetti Carbonara**, whisk eggs and cheese, toss with hot pasta and crispy pancetta, and finish with black pepper. Creamy, rich, and packed with umami flavor!

**Now, generate the summary for the given recipe:**
";

            var result = await kernel.InvokePromptAsync<string>(prompt);
            return result;
        }

        [KernelFunction("GenerateRecipes")]
        [Description("Generates new recipe suggestions based on available ingredients.")]
        public async Task<List<RecipeModel>> GenerateRecipesAsync(string ingredients, Kernel kernel)
        {
            string prompt = $@"
You are a professional chef and AI-powered recipe assistant. 
Given the following ingredients: **{ingredients}**, generate a list of recipes.

### Instructions:
- Suggest **3-5 complete recipes** that include these ingredients.
- Provide a **recipe name, a short ingredient list is given by user, and step-by-step instructions of 2 lines**.
- Keep the recipes practical, simple, and unique.
- Include measurements for key ingredients.
- Ensure variety in the generated recipes.
- Ingredient shoud match {ingredients}
- give the recipe in json formate

### Examples:
[
  {{
    ""id"": ""0""
    ""recipe_name"": ""Albondigas Soup"",
    ""ingredients"": ""Ground beef, Rice, Egg, Garlic, Onion, Cumin, Carrots, Potatoes, Tomato sauce, Cilantro."",
    ""instructions"": ""Mix ground beef with rice, egg, garlic, onion, and cumin. Shape into meatballs. Simmer in tomato sauce with carrots and potatoes. Garnish with cilantro.""
  }},
  {{
    ""id"": ""0""
    ""recipe_name"": ""Albondigas Soup"",
    ""ingredients"": ""Ground beef, Rice, Egg, Garlic, Onion, Cumin, Carrots, Potatoes, Tomato sauce, Cilantro."",
    ""instructions"": ""Mix ground beef with rice, egg, garlic, onion, and cumin. Shape into meatballs. Simmer in tomato sauce with carrots and potatoes. Garnish with cilantro.""
  }},
  {{
   ""id"": ""0""
    ""recipe_name"": ""Chiles Rellenos"",
    ""ingredients"": ""Poblano chiles, Cheese or meat filling, Eggs, Flour, Salt, Oil, Tomato sauce."",
    ""instructions"": ""Roast and peel chiles, remove seeds. Stuff with cheese or meat. Separate eggs, beat whites until stiff, fold in yolks. Coat chiles in flour, dip in egg batter, and fry until golden. Serve with tomato sauce.""
  }},
  {{
   ""id"": ""0""
    ""recipe_name"": ""Spanish Omelet (Supreme)"",
    ""ingredients"": ""Bacon, Minced onion, Garlic, Minced green sweet pepper, Canned red sweet pepper, Parsley, Sliced ripe olives, Seedless raisins, Mushrooms, Flour, Tomato (raw or canned), Salt, Chile powder or red chile sauce, Eggs, Water, Cream of tartar, Butter or lard."",
    ""instructions"": ""Fry bacon, remove. Cook onion, garlic, peppers, parsley, olives, raisins, and mushrooms in bacon fat. Add flour, then tomato, salt, and chile powder. Keep hot. Beat egg yolks until light, add water and salt. Beat whites with cream of tartar and fold into yolks. Cook omelet in butter or lard, lifting edges to cook evenly. When nearly done, broil or bake briefly. Fill with prepared mix, fold, and serve garnished with whipped egg whites, pimiento, olives, parsley, bacon, and toast triangles.""
  }}
]
###Note:
remove the ***`json*** tag while printing 
Now, generate the recipes:
";

            var response = await kernel.InvokePromptAsync<string>(prompt);

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

          
        

    }
}
