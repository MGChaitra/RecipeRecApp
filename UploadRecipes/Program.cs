using System.Text.Json;
using Microsoft.SemanticKernel;
using Models;
using RecipeRec.KernelOps.Contracts;
using RecipeRec.KernelOps.KernelProvider;

namespace UploadRecipes
{
	internal class Program
	{
		static void Main(string[] args)
		{

			CreateData().Wait();
		}

		static async Task CreateData()
		{
			try
			{
				int numbers = 5;
				List<RecipeModel> recipes = [];
				IKernalProvider kernalProvider = new KernalProvider();
				var kernel = kernalProvider.CreateKernal();

				var response = kernel.InvokePromptAsync(@$"act as a recipe provider in list of json format based on field values given below:
				```Recipe class fields which act as json keys, you are here to generate respecive key's values starts
						public int Id
						public string Name
						public string Description
						public List<IngredientModel> Ingredients = [provide ingredients needed without quantifying them here Note that ingredients are not just strings, they have their attributes mentioned in next line];
				```Ingredient Model attributes start:
						public int Id  = 'starts from 1 for each recipe'
						public string name  = 'ingredient name';
						public string food_group = 'specifies which group the ingredient belong to, Eg: Egg, Spices, milk and derivates, pulses...';
				```Ingredient Model attributes end;
						public List<string> Instructions  = [provide instructions along with instruction numbers for each starting from 1.];
						public bool IsVeg  = true/false based on ingredients;
				```recipe class fields ends;
				return the response in list of json format only so that i can map that to a classModel and store it in azure ai search index, and don't add any quotes or any extra information not even json
				Provide atleast {numbers}  recipes");

				var result = response.Result.ToString();
				Console.WriteLine(result);
				recipes = JsonSerializer.Deserialize<List<RecipeModel>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
				if (recipes.Count > 0)
				{

					var arguments = new KernelArguments();
					arguments.Add("recipes", recipes);

					var res = await kernel.InvokeAsync("IndexPlugin", "Upload_Recipes", arguments);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
