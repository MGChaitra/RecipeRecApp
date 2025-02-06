using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Models;
using RecipeRec.KernelOps.Contracts;

namespace RecipeRec.KernelOps.Plugins
{
	public class CustomizePlugin(Kernel kernel, IConfiguration configuration) : ICustomizePlugin
	{
		private readonly IConfiguration configuration = configuration;
		private readonly Kernel kernel = kernel;

		[KernelFunction("Custom_instructions")]
		[Description("For every given list of instructions you customize those and return list of custom instructions")]
		public async Task<List<string>> CustomizeInstructions(List<string> instructions)
		{
			List<string> result = [];
			try
			{
				var promptFilePath = configuration["PromptFiles:CustomInstructionPrompt"];
				string prompt = await File.ReadAllTextAsync(promptFilePath!);
				for (int i = 0; i < instructions.Count; i++)
				{
					var arguments = new KernelArguments
					{
						["Number"] = i + 1,
						["Instruction"] = instructions[i]
					};

					FunctionResult CustomInstruction = await kernel.InvokePromptAsync(prompt, arguments);
					result.Add(CustomInstruction.ToString());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
			return result;
		}

		[KernelFunction("custom_recipes")]
		[Description("Ivoke when the user needs ai generated recipes based on ingredients")]
		public async Task<List<RecipeModel>> CustomizeRecipe(List<IngredientModel> selectedIngredients)
		{
			List<RecipeModel> recipes = [];

			try
			{
				string RequiredIngredients = "";
				foreach (var ing in selectedIngredients)
				{
					RequiredIngredients += $"{ing.Name}, ";
				}

				var arguments = new KernelArguments()
				{
					["RequiredIngredients"] = RequiredIngredients,
				};

				var promptFilePath = configuration["PromptFiles:CustomRecipePrompt"];
				string prompt = await File.ReadAllTextAsync(promptFilePath!);
				var response = kernel.InvokePromptAsync(prompt, arguments);

				var result = response.Result.ToString();
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
