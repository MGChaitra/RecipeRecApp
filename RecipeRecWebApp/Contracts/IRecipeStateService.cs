using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IRecipeStateService
    {

        List<RecipeModel> Recipes { get; }
        bool HasRecipes { get; }
        void SetRecipes(List<RecipeModel> recipes);
        void ClearRecipes();
    }
}
