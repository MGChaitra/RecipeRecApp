using Models;

namespace RecipeRecWebApp.Contracts
{
	public interface IFavoritesService
	{
		Task GetAllFavorites();
		Task<string> RemoveFavorite(RecipeModel recipe);
		Task<string> SaveFavorite(RecipeModel recipe);
		Task<string> UpdateFavorites(RecipeModel recipe);
	}
}