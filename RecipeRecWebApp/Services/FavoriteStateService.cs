using Models;

namespace RecipeRecWebApp.Services
{
    public class FavoriteStateService
    {
        public event Action? OnFavoritesChanged;
        private List<FavoriteRecipeModel> _favorites=new();
        public List<FavoriteRecipeModel> FavoriteRecipes=> _favorites;
        public void SetFavorites(List<FavoriteRecipeModel> favorites)
        {
            _favorites = favorites;
            NotifyStateChanged();
        }
        public void AddFavorites(FavoriteRecipeModel favorite)
        {
            _favorites.Add(favorite);
            NotifyStateChanged();
        }
        public void RemoveFavorites(string recipename)
        {
            _favorites.RemoveAll(r=>r.recipe_name==recipename);
            NotifyStateChanged();
        }
        private void NotifyStateChanged()=> OnFavoritesChanged?.Invoke();
    }
}
