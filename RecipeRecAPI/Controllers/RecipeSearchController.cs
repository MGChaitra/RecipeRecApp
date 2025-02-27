using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using RecipeAPIProcessor.Contacts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeSearchController : ControllerBase
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
            try
            {
                var recipes = await _searchService.SearchRecipesAsync(query);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                // Log the exception (assuming you have a logger)
                // _logger.LogError(ex, "Error searching recipes");
                return StatusCode(500, "An error occurred while searching for recipes.");
            }
        }

        [HttpPost("upload-recipes")]
        public async Task<IActionResult> UploadRecipes(List<RecipeModel> recipes)
        {
            try
            {
                bool isSuccess = await _recipeIndexService.UploadRecipesToAzureSearch(recipes);
                if (isSuccess)
                {
                    return Ok("Recipes uploaded successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to upload recipes.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (assuming you have a logger)
                // _logger.LogError(ex, "Error uploading recipes");
                return StatusCode(500, "An error occurred while uploading recipes.");
            }
        }
    }
}
