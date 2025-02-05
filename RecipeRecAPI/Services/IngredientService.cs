using System.Collections.Generic;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Models;
using RecipeRec.KernelOps.Contracts;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Services
{
	public class IngredientService(IWebHostEnvironment webHostEnvironment, ILogger<IngredientService> logger, IKernalProvider kernalProvider) : IIngredientService
	{
		private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
		private readonly ILogger<IngredientService> logger = logger;
		private readonly IKernalProvider kernalProvider = kernalProvider;

		public void AddIngredient(IngredientModel newIngredient)
		{
			logger.LogInformation("Adding Ingredient");
			try
			{
				var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");

				var jsonData = File.ReadAllText(filePath);
				var ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? [];
				var item = ingredients.FirstOrDefault(items => String.Equals(items.Name, newIngredient.Name, StringComparison.OrdinalIgnoreCase));
				if (item == null) ingredients.Add(newIngredient);


				var updatedJson = JsonSerializer.Serialize(ingredients);
				File.WriteAllText(filePath, updatedJson);
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		public List<IngredientModel> GetIngredients()
		{
			List<IngredientModel> ingredients = [];
			try
			{
				var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "FoodDB.json");
				if (!File.Exists(filePath)) return [];

				var jsonData = File.ReadAllText(filePath);
				ingredients = JsonSerializer.Deserialize<List<IngredientModel>>(jsonData) ?? [];
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

			return ingredients;
		}

		public async Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			//Process ingredients return recipes;
			try 
			{
				var kernel = kernalProvider.CreateKernal();
				var arguments = new KernelArguments();
				arguments.Add("selectedIngredients",selectedIngredients);
				var res = await kernel.InvokeAsync("IndexPlugin", "Get_Recipes",arguments);
				var recipes = res.GetValue<List<RecipeModel>>();

				var recipesAugmented = await kernel.InvokeAsync("CustomizePlugin", "custom_recipes", arguments);
				var RecipesFromGPT = recipesAugmented.GetValue<List<RecipeModel>>() ?? [];
				foreach(var recipe in recipes!)
				{
					RecipesFromGPT.Add(recipe);
				}
				return RecipesFromGPT!;
			}
			catch(Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return [];
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
			catch(Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return instructions;
		}
	}
}
