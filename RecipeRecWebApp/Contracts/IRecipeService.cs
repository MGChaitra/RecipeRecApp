using Models;

namespace RecipeRecWebApp.Contracts
{
	public interface IRecipeService
	{
		Task<List<string>> CustomInstructions(RecipeModel recipe);
		Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients);
	}
}