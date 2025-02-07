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
				return NotFound("Ingredient not found");
			}
			ingredientService.AddIngredient(newIngredient);
			return Ok("Ingredient Added");

		}
	}
}
