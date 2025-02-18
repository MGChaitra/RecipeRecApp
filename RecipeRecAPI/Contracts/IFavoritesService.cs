using Models;

namespace RecipeRecAPI.Contracts
{
	public interface IFavoritesService
	{
		Task<string> DeleteFavorites(string id);
		Task<List<RecipeModel>> GetAllRecipes();
		Task<string> SaveFavorites(RecipeModel recipe);
		Task<string> UpdateFavorites(RecipeModel recipe);
	}
}