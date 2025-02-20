using Models;
using RecipeRecWebApp.Pages;

namespace RecipeRecWebApp.Contracts
{
    public interface IFavoriteStateService
    {
        event Action? OnFavoritesChanged;
        List<FavoriteRecipeModel> FavoriteRecipes { get; }
        void SetFavorites(List<FavoriteRecipeModel> favorites);
        void AddFavorites(FavoriteRecipeModel favorite);
        void RemoveFavorites(string recipename);
        void NotifyStateChanged();
    }
}
