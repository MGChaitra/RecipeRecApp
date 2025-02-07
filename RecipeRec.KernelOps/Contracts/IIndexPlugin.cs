using Models;

namespace RecipeRec.KernelOps.Contracts
{
	public interface IIndexPlugin
	{
		Task createRecipeIndex();
		Task<List<RecipeModel>> GetRecipes(List<string> expandedIngredients);
		Task UploadRecipes(List<RecipeModel> recipes);
	}
}