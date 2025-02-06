using Models;

namespace RecipeRecWebApp.Contracts
{
    public interface IRecipeSearchService
    {
        Task<List<RecipeModel>> SearchRecipesAsync(List<string> ingredients);
        Task<List<RecipeModel>> GenerateRecipesAsync(List<string> ingredients);
        Task<string?> SummarizeRecipeAsync(RecipeModel recipe);
    }
}
