using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using RecipeAPIProcessor.Contacts;

namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeSearchController : Controller
    {

        private readonly IUploadToIndexService _recipeIndexService;
        private readonly IAzureAISearchService _searchService;

        public RecipeSearchController(IAzureAISearchService searchService, IUploadToIndexService recipeIndexService)
        {
          _recipeIndexService = recipeIndexService;
            _searchService = searchService;
        }

     
        [HttpGet("search")]
        public async Task<IActionResult> SearchRecipes([FromQuery] string query)
        {
            var recipes = await _searchService.SearchRecipesAsync(query);
            return Ok(recipes);
        }


        [HttpPost("upload-recipes")]
        public async Task<IActionResult> UploadRecipes(List<RecipeModel> recipes)
        {
           
            bool isSuccess = await _recipeIndexService.UploadRecipesToAzureSearch(recipes);
            return isSuccess ? Ok("Recipes uploaded successfully.") : StatusCode(500, "Failed to upload recipes.");
        }
    }
}
