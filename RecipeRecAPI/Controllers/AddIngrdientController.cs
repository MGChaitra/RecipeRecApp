using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RecipeRecWebApp.Models;

namespace RecipeRecAPI.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/[controller]")]
    public class AddIngrdientController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddIngrdientController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult GetIngredients()
        {
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var jsonData = System.IO.File.ReadAllText(filePath);
            var ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData);
            return Ok(ingredients);
        }
        [HttpPost]
        public Task<IActionResult> SaveFile([FromBody] IngredientModel newIngredient)
        {

            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");
            if (!System.IO.File.Exists(filePath))
                return Task.FromResult<IActionResult>(NotFound());

            var jsonData = System.IO.File.ReadAllText(filePath);
            var ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? new List<IngredientModel>();

            ingredients.Add(newIngredient);
            var updatedJson = JsonSerializer.Serialize(ingredients);

            System.IO.File.WriteAllText(filePath, updatedJson);
            return Task.FromResult<IActionResult>(CreatedAtAction(nameof(GetIngredients), new { id = newIngredient.Id }, newIngredient));

        }
    }
}
