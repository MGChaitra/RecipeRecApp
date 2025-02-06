using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IRecipeSearchService
    {
        void SendIngredients(IngredientModel ingredient);
    }
}
