using Models;
using RecipeRecWebApp.Pages;

namespace RecipeRecWebApp.Contracts
{
    /// <summary>
    /// Defines methods and events for managing the state of favorite recipes, including 
    /// retrieving, setting, adding, and removing favorite recipes, as well as notifying when the state changes.
    /// </summary>
    public interface IFavoriteStateService
    {
        /// <summary>
        /// Event triggered when the favorites list changes.
        /// </summary>
        event Action? OnFavoritesChanged;

        /// <summary>
        /// Gets the list of favorite recipes.
        /// </summary>
        List<FavoriteRecipeModel> FavoriteRecipes { get; }

        /// <summary>
        /// Sets the list of favorite recipes.
        /// </summary>
        /// <param name="favorites">The list of favorite recipes to set.</param>
        void SetFavorites(List<FavoriteRecipeModel> favorites);

        /// <summary>
        /// Adds a recipe to the favorites list.
        /// </summary>
        /// <param name="favorite">The recipe to add to favorites.</param>
        void AddFavorites(FavoriteRecipeModel favorite);

        /// <summary>
        /// Removes a recipe from the favorites list by name.
        /// </summary>
        /// <param name="recipename">The name of the recipe to remove from favorites.</param>
        void RemoveFavorites(string recipename);

        /// <summary>
        /// Notifies that the state of the favorites list has changed.
        /// </summary>
        void NotifyStateChanged();
    }
}
