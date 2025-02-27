using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using RecipeAPIProcessor.Contacts;


namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [ApiController]
    public class CosmosController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        public CosmosController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe([FromBody] FavoriteRecipeModel recipe)
        {
            await _cosmosDbService.AddRecipeAsync(recipe);
            return Ok(new { message = "Recipe added successfully!" });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            await _cosmosDbService.DeleteRecipeAsync(id);
            return Ok(new { message = "deleted successfully" });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _cosmosDbService.GetRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{recipeName}")]
        public async Task<IActionResult> GetRecipeByName(string recipeName)
        {
            var recipe = await _cosmosDbService.GetRecipeByNameAsync(recipeName);
            if (recipe == null)
            {
                return NotFound(new { message = "Recipe not found!" });
            }
            return Ok(recipe);
        }
    }
}
