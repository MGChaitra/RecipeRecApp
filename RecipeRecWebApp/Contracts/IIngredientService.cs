using Models;

namespace RecipeRecWebApp.Contracts
{
	public interface IIngredientService
	{
		Task<bool> AddIngredientAsync(IngredientModel ingredient);
		Task<List<IngredientModel>> GetIngredientsAsync();
		void MapIngredients();
	}
}