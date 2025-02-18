using Microsoft.AspNetCore.Mvc;
using Models;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FavoritesController(IFavoritesService favoritesService) : ControllerBase
	{
		private readonly IFavoritesService favoritesService = favoritesService;

		[HttpPost]
		public async Task<IActionResult> SaveFavorites(RecipeModel recipe)
		{
			if (recipe == null)
			{
				return NotFound("Recipe not found to save");
			}
			return Ok(await favoritesService.SaveFavorites(recipe));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFavorite(string id)
		{
			if (String.IsNullOrEmpty(id))
			{
				return NotFound("Recipe not found to unsave");
			}
			return Ok(await favoritesService.DeleteFavorites(id));
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateFavoriteRecipe(RecipeModel recipe)
		{
			if (recipe == null)
			{
				return NotFound("Recipe not found to update");
			}
			return Ok(await favoritesService.UpdateFavorites(recipe));
		}

		[HttpGet]
		public async Task<IActionResult> GetFavoriteList()
		{
			return Ok(await favoritesService.GetAllRecipes());
		}
	}
}
