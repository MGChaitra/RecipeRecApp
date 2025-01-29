using Models;

namespace RecipeRecAPI.Contracts
{
    public interface IIngredientService
    {
        List<IngredientModel> GetIngredients();
        bool AddIngredient(IngredientModel newIngredient);
    }
}
