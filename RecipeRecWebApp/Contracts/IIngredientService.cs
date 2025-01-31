using Models;

namespace RecipeRecWebApp.Contracts
{
	public interface IIngredientService
	{
		Task<bool> AddIngredientAsync(IngredientModel ingredient);
		Task<List<IngredientModel>> GetIngredientsAsync();
		Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients);
		void MapIngredients();
	}
}