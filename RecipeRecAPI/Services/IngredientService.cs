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
				recipes?.Add(new RecipeModel
				{
					Id = 1,
					Name = "Egg Omelet",
					Description = "Cooked Flat Baked Egg Omelet",
					IsVeg = false,
					Instructions = ["1) Egg Breaking", "2) Pan heating", "3) Pour egg"],
					AdditionalRequiredIngredients = [],
					RequiredIngredients = selectedIngredients,
					ExtraSelectedIngredients = []
				});

				return recipes!;
			}
			catch(Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return [];
		}
	}
}
