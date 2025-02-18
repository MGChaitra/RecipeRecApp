
using Microsoft.AspNetCore.Mvc;
using Models;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RecipesController(IRecipeServices recipeServices) : ControllerBase
	{
		private readonly IRecipeServices recipeServices = recipeServices;

		[HttpPost("GetRecipes")]
		public async Task<IActionResult> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			if (selectedIngredients == null || selectedIngredients.Count == 0)
			{
				return NotFound("Ingredients Not Found");
			}
			return Ok(await recipeServices.GetRecipes(selectedIngredients));
		}

		[HttpPost("CustomizeInstructions")]
		public async Task<IActionResult> GetCustomInstructions(RecipeModel recipe)
		{
			if (recipe == null)
			{
				return NotFound("recipe not found");
			}
			return Ok(await recipeServices.CustomInstructions(recipe));
		}
	}
}
