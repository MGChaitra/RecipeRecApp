using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IngredientController(IIngredientService ingredientService) : ControllerBase
	{
		private readonly IIngredientService ingredientService = ingredientService;

		[HttpGet]
		public IActionResult GetIngredients()
		{
			return Ok(ingredientService.GetIngredients());
		}

		[HttpPost]
		public IActionResult AddIngredient([FromBody] IngredientModel newIngredient)
		{
			if(String.IsNullOrEmpty(newIngredient.Name) || newIngredient == null)
			{
				return BadRequest("Ingredient is null");
			}
			ingredientService.AddIngredient(newIngredient);
			return Ok("Ingredient Added");

		}

		[HttpPost("GetRecipes")]
		public async Task<IActionResult> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			return Ok(await ingredientService.GetRecipes(selectedIngredients));
		}

		[HttpPost("CustomizeInstructions")]
		public async Task<IActionResult> GetCustomInstructions(RecipeModel recipe)
		{
			if (recipe == null)
			{
				return BadRequest("recipe is null");
			}
			return Ok(await ingredientService.CustomInstructions(recipe));
		}
	}
}
