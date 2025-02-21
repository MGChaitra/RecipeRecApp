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

		/// <summary>
		/// Recipes are provided to user based on the selectedIngredients, by LLM utilizing RAG techniques.
		/// </summary>
		/// <param name="selectedIngredients">List of Ingredients as context to LLM for generating relevent recipe</param>
		/// <returns>List of Recipes - RAG recipes</returns>
		public async Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			List<RecipeModel> recipesFromIndex = [];
			try
			{
				var kernel = kernalProvider.CreateKernal();

				List<string> ingredients = [];
				foreach(var item in selectedIngredients)
				{
					ingredients.Add(item.Name);
				}

				//retrieval 
				var arguments = new KernelArguments();
				arguments.Add("Ingredients", ingredients);

				var res = await kernel.InvokeAsync("IndexPlugin", "Get_Recipes", arguments);
				recipesFromIndex = res.GetValue<List<RecipeModel>>() ?? [];

				logger.LogWarning($"Recipes from Index: {recipesFromIndex.Count}");

				//augmentation
				arguments.Clear();
				arguments.Add("selectedIngredients", selectedIngredients);
				arguments.Add("RetrivedRecipes", recipesFromIndex);

				var resAugmented = await kernel.InvokeAsync("CustomizePlugin", "Rag_indexRecipes", arguments);
				recipesFromIndex = resAugmented.GetValue<List<RecipeModel>>() ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

			return recipesFromIndex;
		}

		/// <summary>
		/// Customization of instructions using LLM for a specific recipe.
		/// </summary>
		/// <param name="recipe">RecipeModel, whose instructions has to be cusomized.</param>
		/// <returns>List of string - custom instructions for the existing recipe</returns>
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
