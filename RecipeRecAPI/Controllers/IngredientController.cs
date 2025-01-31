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
			ingredientService.AddIngredient(newIngredient);
			return Ok("Ingredient Added");

		}

		[HttpPost("GetRecipes")]
		public async Task<IActionResult> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			return Ok(await ingredientService.GetRecipes(selectedIngredients));
		}
	}
}
