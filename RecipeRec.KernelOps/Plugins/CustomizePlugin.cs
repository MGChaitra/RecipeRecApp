using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Models;
using RecipeRec.KernelOps.Contracts;
using RecipeRec.KernelOps.KernelProvider;

namespace RecipeRec.KernelOps.Plugins
{
	public class CustomizePlugin(Kernel kernel)
	{
		private readonly Kernel kernel = kernel;

		[KernelFunction("Custom_instructions")]
		[Description("for every given list of instructions you customize those and return list of custom instructions")]
		public async Task<List<string>> CustomizeInstructions(List<string> instructions)
		{
			List<string> result = [];
			for(int i=0;i<instructions.Count;i++)
			{
				FunctionResult CustomInstruction = await kernel.InvokePromptAsync($@"You are a helpful custom recipe instruction provider:
				-given the pre existing instruction: {instructions[i]}, customize the provided instruction, 
				-string represents a recipe prepration instruction along with instruction number: {i+1}
				-Just respond with cutomized instruction and instruction number, without any additional details.");
				Console.Write(CustomInstruction);
				result.Add(CustomInstruction.ToString());
			}
			return result;
		}

		[KernelFunction("custom_recipes")]
		[Description("ivoke when the user needs ai generated recipes based on ingredients")]
		public List<RecipeModel> CustomizeRecipe(List<IngredientModel> selectedIngredients)
		{
			List<RecipeModel> recipes = [];
			IKernalProvider kernalProvider = new KernalProvider();
			var kernel = kernalProvider.CreateKernal();
			string RequiredIngredients = "";
			foreach(var ing in selectedIngredients)
			{
				RequiredIngredients += $"{ing.Name}, ";
			}
			var arguments = new KernelArguments();
			arguments.Add("Ingredients", RequiredIngredients);

			var response = kernel.InvokePromptAsync(@$"act as a recipe provider in list of json format based on field values given below and for given Ingredients: {RequiredIngredients}: 
```Recipe class fields which act as json keys, you are here to generate respecive key's values starts
		public int Id
		public string Name
		public string Description
		public List<IngredientModel> Ingredients = [provide ingredients needed without quantifying them here Note that ingredients are not just strings, they have their attributes mentioned in next line, find recipe based on {selectedIngredients}];
```Ingredient Model attributes start:
		public int Id = 'starts from 1 for each recipe'
		public string name = 'ingredient name'; [note that name should be present in {RequiredIngredients}]
		public string food_group = 'specifies which group the ingredient belong to, Eg: Egg, Spices, milk and derivates, pulses...';
```Ingredient Model attributes end;
		public List<string> Instructions = [provide instructions to prepare the recipe along with instruction numbers for each instruction];
		public bool IsVeg = true/false based on ingedients;
```recipe class fields ends;
```Important step: return the response in list of json format only, and don't even add any kind of quotes or any extra information not even json
			try getting most recipe with provided Ingredients, in case no such recipe exists return empty list only",arguments);

			var result = response.Result.ToString();
			Console.WriteLine(result);
			try
			{
				recipes = JsonSerializer.Deserialize<List<RecipeModel>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}

			return recipes;
		}
	}
}
