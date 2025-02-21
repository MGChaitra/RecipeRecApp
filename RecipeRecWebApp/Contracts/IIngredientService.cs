using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IIngredientService
    {
        /// <summary>
        /// Retrieves a list of all ingredients.
        /// </summary>
        /// <returns>List of ingredients.</returns>
        Task<List<IngredientModel>> GetIngredientsAsync();

        /// <summary>
        /// Adds a new ingredient.
        /// </summary>
        /// <param name="ingredient">The ingredient to add.</param>
        /// <returns>Boolean indicating success.</returns>
        Task<bool> AddIngredientAsync(IngredientModel ingredient);
    }
}
