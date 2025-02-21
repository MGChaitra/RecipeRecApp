using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IRecipeStateService
    {
        /// <summary>
        /// Gets the current list of recipes.
        /// </summary>
        List<RecipeModel> Recipes { get; }

        /// <summary>
        /// Indicates whether there are any recipes available.
        /// </summary>
        bool HasRecipes { get; }

        /// <summary>
        /// Sets the list of recipes.
        /// </summary>
        /// <param name="recipes">The list of recipes to set.</param>
        void SetRecipes(List<RecipeModel> recipes);

        /// <summary>
        /// Clears the current list of recipes.
        /// </summary>
        void ClearRecipes();
    }
}
