using Models;
using RecipeRecWebApp.Contracts;
namespace RecipeRecWebApp.Services
{
    public class RecipeStateService: IRecipeStateService
    {
        public List<RecipeModel> Recipes { get; private set; } = new();
        public bool HasRecipes => Recipes.Count > 0;
        public void SetRecipes(List<RecipeModel> recipes)
        {
            Recipes = recipes;
        }

        public void ClearRecipes()
        {
            Recipes.Clear();
        }
    }
}
