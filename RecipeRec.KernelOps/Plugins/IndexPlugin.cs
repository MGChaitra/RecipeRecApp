using System.ComponentModel;
using System.Text.Json;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Models;
using RecipeRec.AzureAiSearch.IndexCreation.Services;
using RecipeRec.KernelOps.Contracts;

namespace RecipeRec.KernelOps.Plugins
{
	public class IndexPlugin(SearchIndexClient searchIndexClient, SearchClient searchClient, IConfiguration configuration) : IIndexPlugin
	{
		private readonly SearchIndexClient searchIndexClient = searchIndexClient;
		private readonly IConfiguration configuration = configuration;
		private readonly SearchClient searchClient = searchClient;

		[KernelFunction("CreateAzureSearchIndex")]
		[Description("creates Recipe Index in azure search")]
		public async Task createRecipeIndex()
		{
			try
			{
				Console.WriteLine("Info: Creating/Updating Index....");
				var fieldBuilder = new FieldBuilder();
				var fields = fieldBuilder.Build(typeof(RecipeIndexModel));
				var addScoringProfile = new AddScoringProfile(configuration);
				var addVectorProfile = new AddVectorProfile(configuration);
				var indexClient = searchIndexClient;

				var scoringProfile = addScoringProfile.Add();
				var vectorSearch = addVectorProfile.Add();


				var index = new SearchIndex(configuration["SearchClient:index"])
				{
					Fields = fields,
					VectorSearch = vectorSearch
				};
				index.ScoringProfiles.Add(scoringProfile);

				await indexClient.CreateOrUpdateIndexAsync(index);
				Console.WriteLine("Info: Index Created/Updated");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}

		}

		[KernelFunction("Get_Recipes")]
		[Description("returns the list of recipes based on provided list of ingredients ")]
		public async Task<List<RecipeModel>> GetRecipes(List<IngredientModel> selectedIngredients)
		{
			//var options = new SearchOptions {

			//};
			//var res = await searchClient.SearchAsync<SearchDocument>("",options);
			//var response = res.GetRawResponse().Content.ToString();
			//var list = JsonSerializer.Serialize<List<RecipeModel>>(response);
			await Task.Delay(1000);
			return new List<RecipeModel>();

		}
	}
}
