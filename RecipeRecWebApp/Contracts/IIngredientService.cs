using RecipeRecWebApp.Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IIngredientService
    {
        Task<List<IngredientModel>> GetIngredientsAsync();
        Task<bool> AddIngredientAsync(IngredientModel ingredient);
    }
}
