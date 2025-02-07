using Models;

namespace RecipeRecAPI.Contracts
{
	public interface IRecipeServices
	{
		Task<List<string>> CustomInstructions(RecipeModel recipe);
		Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients);
	}
}