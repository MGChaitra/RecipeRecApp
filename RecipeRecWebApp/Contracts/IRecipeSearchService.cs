using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IRecipeSearchService
    {
        /// <summary>
        /// Searches for recipes that match the given ingredients.
        /// </summary>
        /// <param name="ingredients">List of ingredients to search for.</param>
        /// <returns>List of matching recipes.</returns>
        Task<List<RecipeModel>> SearchRecipesAsync(List<string> ingredients);

        /// <summary>
        /// Generates new recipes based on the given ingredients.
        /// </summary>
        /// <param name="ingredients">List of ingredients to generate recipes from.</param>
        /// <returns>List of generated recipes.</returns>
        Task<List<RecipeModel>> GenerateRecipesAsync(List<string> ingredients);

        /// <summary>
        /// Summarizes the given recipe.
        /// </summary>
        /// <param name="recipe">The recipe to summarize.</param>
        /// <returns>Summarized version of the recipe.</returns>
        Task<SummarizedRecipeModel?> SummarizeRecipeAsync(RecipeModel recipe);

        /// <summary>
        /// Stores the given list of recipes.
        /// </summary>
        /// <param name="recipes">List of recipes to store.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StoreRecipeAsync(List<RecipeModel> recipes);
    }
}
