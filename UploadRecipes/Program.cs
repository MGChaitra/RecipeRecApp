using System.Text.Json;
using Microsoft.Extensions.Configuration;
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
			IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			CreateData(configuration).Wait();
		}

		static async Task CreateData(IConfiguration configuration)
		{
			try
			{
				List<RecipeModel> recipes = [];
				string numberOfRecipes = configuration["IndexSettings:NumberOfRecipes"]!;
				
				IKernalProvider kernalProvider = new KernalProvider();
				var kernel = kernalProvider.CreateKernal();

				string promptFilePath = configuration["PromptFiles:RecipeGenerationPrompt"]!;
				var prompt = await File.ReadAllTextAsync(promptFilePath);
				var promptExecutionSettings = kernalProvider.RequiredSettings();
				var arguments = new KernelArguments
				{
					["Numbers"] = numberOfRecipes,
				};

				var response = kernel.InvokePromptAsync(prompt,arguments);

				var result = response.Result.ToString();
				recipes = JsonSerializer.Deserialize<List<RecipeModel>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
				if (recipes.Count > 0)
				{

					var FunctionArguments = new KernelArguments();
					FunctionArguments.Add("recipes", recipes);

					var res = await kernel.InvokeAsync("IndexPlugin", "Upload_Recipes", FunctionArguments);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
