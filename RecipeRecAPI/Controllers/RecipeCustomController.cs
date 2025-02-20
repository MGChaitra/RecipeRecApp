using System.ComponentModel;
using System.Linq.Expressions;
using Azure.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.SemanticKernel;
using Models;
using RecipeRecAPI.Plugins;
using RecipeRecAPI.Services;

namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeCustomController : Controller
    {
        private readonly RecipeCustomPlugin _recipeCustomPlugin;
        private readonly Kernel _kernel;
        public RecipeCustomController(RecipeCustomPlugin recipeCustomPlugin, Kernel kernel)
        {
            _kernel = kernel;
            _recipeCustomPlugin = recipeCustomPlugin;
        }
        [HttpPost("summarize")]
        public async Task<IActionResult> GenerateRecipeSummaries([FromBody] List<RecipeModel> recipes)
        {
            if (recipes == null || recipes.Count == 0)
                return BadRequest("No recipes provided.");
            var enhancedRecipes = new List<SummarizedRecipeModel>();

            foreach (var recipe in recipes)
            {
                string fullrecipe = $"{recipe.recipe_name}\nInstructions: {recipe.instructions}";
                var summary = await _recipeCustomPlugin.SummaryRecipeAsync(fullrecipe, _kernel);

                foreach (var sum in summary)
                {
                    enhancedRecipes.Add(new SummarizedRecipeModel
                    {
                        Title = sum.Title,
                        Ingredients =sum.Ingredients,
                        Summary = sum.Summary,

                    });
                }
            }

            return Ok(enhancedRecipes);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateRecipesFromAI([FromBody] List<string> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0)
                return BadRequest("No ingredients provided.");

            var query = string.Join(", ", ingredients);
            var recipes = await _recipeCustomPlugin.GenerateRecipesAsync(query, _kernel);

            if (recipes == null || recipes.Count == 0)
                return NotFound("No recipes generated.");

            return Ok(recipes);
        }

    }
}
