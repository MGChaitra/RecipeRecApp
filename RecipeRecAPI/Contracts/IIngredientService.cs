using Models;

namespace RecipeRecAPI.Contracts
{
    /// <summary>
    /// Defines methods for managing ingredients, including retrieving and adding ingredients.
    /// </summary>
    public interface IIngredientService
    {
        /// <summary>
        /// Retrieves a list of all ingredients.
        /// </summary>
        /// <returns>A list of <see cref="IngredientModel"/> instances.</returns>
        List<IngredientModel> GetIngredients();

        /// <summary>
        /// Adds a new ingredient to the list.
        /// </summary>
        /// <param name="newIngredient">The new ingredient to add.</param>
        /// <returns>A boolean indicating whether the addition was successful.</returns>
        bool AddIngredient(IngredientModel newIngredient);
    }
}
