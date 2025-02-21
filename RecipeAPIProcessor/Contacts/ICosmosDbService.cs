using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RecipeAPIProcessor.Contacts
{
    /// <summary>
    /// Defines methods for interacting with a Cosmos DB, including adding, deleting, 
    /// retrieving a recipe by name, and retrieving all recipes.
    /// </summary>
    public interface ICosmosDbService
    {
        /// <summary>
        /// Asynchronously adds a recipe to the database.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        Task AddRecipeAsync(FavoriteRecipeModel recipe);

        /// <summary>
        /// Asynchronously deletes a recipe from the database by name.
        /// </summary>
        /// <param name="recipeName">The name of the recipe to delete.</param>
        Task DeleteRecipeAsync(string recipeName);

        /// <summary>
        /// Asynchronously retrieves a recipe from the database by name.
        /// </summary>
        /// <param name="recipeName">The name of the recipe to retrieve.</param>
        /// <returns>The recipe with the specified name.</returns>
        Task<FavoriteRecipeModel> GetRecipeByNameAsync(string recipeName);

        /// <summary>
        /// Asynchronously retrieves all recipes from the database.
        /// </summary>
        /// <returns>A list of all recipes.</returns>
        Task<List<FavoriteRecipeModel>> GetRecipesAsync();
    }
}
