using Models;

namespace RecipeRecAPI.Contracts
{
	public interface IIngredientService
	{
		void AddIngredient(IngredientModel newIngredient);
		List<IngredientModel> GetIngredients();
	}
}