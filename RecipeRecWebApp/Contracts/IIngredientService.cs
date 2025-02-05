using Models;

namespace RecipeRecWebApp.Contracts
{
	public interface IIngredientService
	{
		Task<List<string>> CustomInstructions(RecipeModel recipe);
		Task<bool> AddIngredientAsync(IngredientModel ingredient);
		Task<List<IngredientModel>> GetIngredientsAsync();
		Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients);
		void MapIngredients();
	}
}