using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeSearchController : Controller
    {

       
        private readonly AzureAISearchService _searchService;

        public RecipeSearchController(AzureAISearchService searchService)
        {
          
            _searchService = searchService;
        }

     
        [HttpGet("search")]
        public async Task<IActionResult> SearchRecipes([FromQuery] string query)
        {
            var recipes = await _searchService.SearchRecipesAsync(query);
            return Ok(recipes);
        }
    }
}
