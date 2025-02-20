using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface ICosmosDbService
    {
        Task<List<FavoriteRecipeModel>> GetAllFavRecipe();
        Task<bool> AddToFavorites(FavoriteRecipeModel recipe);
        Task<bool> RemoveFromFv(string recipename);
    }
}
