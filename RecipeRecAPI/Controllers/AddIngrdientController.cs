using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeRecAPI.Contracts;
using Models;

namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class AddIngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;

        public AddIngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public IActionResult GetIngredients()
        {
            var ingredients = _ingredientService.GetIngredients();
            if (ingredients == null)
                return NotFound();

            return Ok(ingredients);
        }

        [HttpPost]
        public IActionResult SaveFile([FromBody] IngredientModel newIngredient)
        {
            if (newIngredient == null)
                return BadRequest("Invalid ingredient data");
            var success = _ingredientService.AddIngredient(newIngredient);
            if (!success)
                return NotFound("Data file not found");
            return CreatedAtAction(nameof(GetIngredients), new { id = newIngredient.Id }, newIngredient);
        }
    }

}