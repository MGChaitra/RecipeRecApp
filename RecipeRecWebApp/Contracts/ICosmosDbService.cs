using Models;

namespace RecipeRecWebApp.Contracts
{
    /// <summary>
    /// Defines methods for interacting with a Cosmos DB, including retrieving all favorite recipes, 
    /// adding a recipe to favorites, and removing a recipe from favorites.
    /// </summary>
    public interface ICosmosDbService
    {
        /// <summary>
        /// Asynchronously retrieves all favorite recipes from the database.
        /// </summary>
        /// <returns>A list of <see cref="FavoriteRecipeModel"/> instances.</returns>
        Task<List<FavoriteRecipeModel>> GetAllFavRecipe();

        /// <summary>
        /// Asynchronously adds a recipe to the favorites in the database.
        /// </summary>
        /// <param name="recipe">The recipe to add to favorites.</param>
        /// <returns>A boolean indicating whether the addition was successful.</returns>
        Task<bool> AddToFavorites(FavoriteRecipeModel recipe);

        /// <summary>
        /// Asynchronously removes a recipe from the favorites in the database by name.
        /// </summary>
        /// <param name="recipename">The name of the recipe to remove from favorites.</param>
        /// <returns>A boolean indicating whether the removal was successful.</returns>
        Task<bool> RemoveFromFv(string recipename);
    }
}
