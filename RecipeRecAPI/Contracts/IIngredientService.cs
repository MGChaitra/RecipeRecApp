using Models;

namespace RecipeRecAPI.Contracts
{
	public interface IIngredientService
	{
		Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredient);
		void AddIngredient(IngredientModel newIngredient);
		List<IngredientModel> GetIngredients();
	}
}