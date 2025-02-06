using Models;

namespace RecipeRec.KernelOps.Contracts
{
	public interface ICustomizePlugin
	{
		Task<List<string>> CustomizeInstructions(List<string> instructions);
		Task<List<RecipeModel>> CustomizeRecipe(List<IngredientModel> selectedIngredients);
	}
}