using Microsoft.SemanticKernel;
using Models;
using RecipeRec.KernelOps.Contracts;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Services
{
	public class RecipeServices(ILogger<RecipeServices> logger, IKernalProvider kernalProvider) : IRecipeServices
	{
		private readonly ILogger<RecipeServices> logger = logger;
		private readonly IKernalProvider kernalProvider = kernalProvider;

		public async Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			List<RecipeModel> recipesFromIndex = [];
			try
			{

				List<string> expandedIngredients = selectedIngredients.SelectMany(ing => new List<string> { ing.Name.ToLower(), $"{ing.Name.ToLower()}s" }).Distinct().ToList();

				var kernel = kernalProvider.CreateKernal();

				var arguments = new KernelArguments();
				arguments.Add("expandedIngredients", expandedIngredients);

				var res = await kernel.InvokeAsync("IndexPlugin", "Get_Recipes", arguments);
				recipesFromIndex = res.GetValue<List<RecipeModel>>() ?? [];
				logger.LogWarning($"Recipes from Index: {recipesFromIndex.Count}");

				List<RecipeModel> RecipesFromAI = [];

				arguments.Clear();
				arguments.Add("selectedIngredients", selectedIngredients);
				var recipesAugmented = await kernel.InvokeAsync("CustomizePlugin", "custom_recipes", arguments);
				RecipesFromAI = recipesAugmented.GetValue<List<RecipeModel>>() ?? [];
				foreach (var recipe in RecipesFromAI)
				{
					recipesFromIndex.Add(recipe);
				}

			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

			return recipesFromIndex;
		}

		public async Task<List<string>> CustomInstructions(RecipeModel recipe)
		{
			List<string> instructions = [];
			try
			{
				var kernel = kernalProvider.CreateKernal();
				var arguments = new KernelArguments();
				arguments.Add("instructions", recipe.Instructions);

				var res = await kernel.InvokeAsync("CustomizePlugin", "Custom_instructions", arguments);
				instructions = res.GetValue<List<string>>() ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return instructions;
		}
	}
}
