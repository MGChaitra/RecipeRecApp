using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Models;
using RecipeRec.AzureAiSearch.IndexCreation.Services;
using RecipeRec.KernelOps.Contracts;

namespace RecipeRec.KernelOps.Plugins
{
	public class IndexPlugin(SearchIndexClient searchIndexClient, SearchClient searchClient, IConfiguration configuration,Kernel kernel) : IIndexPlugin
	{
		private readonly SearchIndexClient searchIndexClient = searchIndexClient;
		private readonly IConfiguration configuration = configuration;
		private readonly SearchClient searchClient = searchClient;
		private readonly Kernel kernel = kernel;
		private readonly ILogger<IndexPlugin> logger = kernel.LoggerFactory.CreateLogger<IndexPlugin>();


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
				logger.LogInformation("Index Created/Updated");
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

		}

		[KernelFunction("Get_Recipes")]
		[Description("returns the list of recipes based on provided list of ingredients ")]
		public async Task<List<RecipeModel>> GetRecipes(List<string> expandedIngredients)
		{
			List<RecipeModel> returnList = [];
			try
			{
				int N = Convert.ToInt32(configuration["SearchClient:RecipeNumber"]);
				Response<long> countResponse = await searchClient.GetDocumentCountAsync();
				int TotalCount = (int)Math.Min(countResponse.Value, N);

				string QueryIngredient = string.Join(", ", expandedIngredients);

				#pragma warning disable SKEXP0010 
				var IngredientsEmbedding = await kernel.GetRequiredService<AzureOpenAITextEmbeddingGenerationService>().GenerateEmbeddingsAsync([QueryIngredient]);
				#pragma warning restore SKEXP0010 


				string? filterQuery = $"Ingredients/any(i: {string.Join(" or ",expandedIngredients.Select(ing => $"i eq '{ing.ToLower()}'"))})";


				SearchOptions searchOptions = new SearchOptions()
				{
					QueryType = SearchQueryType.Full,
					VectorSearch = new()
					{
						Queries = { new VectorizedQuery(IngredientsEmbedding[0]) { KNearestNeighborsCount = 4, Fields = { "RecipeEmbeddings" } } }
					},
					Size = TotalCount
					//Filter = filterQuery
				};
				var result = await searchClient.SearchAsync<SearchDocument>(QueryIngredient, options: searchOptions);

				var SearchResponse = result.GetRawResponse().Content.ToString();
				JsonObject jsonValue = JsonSerializer.Deserialize<JsonObject>(SearchResponse)!;
				JsonArray jsonArray = jsonValue?["value"]?.AsArray()!;
				foreach (var jsonObject in jsonArray)
				{
					var recipe = new RecipeModel();
					recipe.Name = jsonObject!["Name"]!.GetValue<string>();
					recipe.Description = jsonObject!["Description"]!.GetValue<string>();
					recipe.IsVeg = jsonObject!["IsVeg"]!.GetValue<bool>();
					var instArray = jsonObject!["Instructions"]!.AsArray();
					List<string> inst = [];
					foreach (var instItem in instArray)
					{
						inst.Add(instItem!.GetValue<string>());
					}
					recipe.Instructions = inst;
					//var ingrArray = jsonObject!["Ingredients"]!.AsArray();
					//List<IngredientModel> ingre = [];
					//foreach (var ingred in ingrArray)
					//{
					//	var ingrepush = new IngredientModel();
					//	var name = ingred!.GetValue<string>();
					//	ingrepush.Name = name;
					//	ingre.Add(ingrepush);
					//}
					//recipe.Ingredients = ingre;
					returnList.Add(recipe);
				}
			}
			catch(Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

			return returnList ?? [];

		}

		[KernelFunction("Upload_Recipes")]
		[Description("Uploads the List of Recipes provided by user to azure search index after embedding recipes")]
		public async Task UploadRecipes(List<RecipeModel> recipes)
		{
			try
			{
				List<string> recipesList = [];
				foreach (var recipe in recipes)
				{
					string val = JsonSerializer.Serialize(recipe);
					recipesList.Add(val);
				}
				IList<ReadOnlyMemory<float>> embeddings = [];
				#pragma warning disable SKEXP0010
				var service = kernel.GetRequiredService<AzureOpenAITextEmbeddingGenerationService>();
				#pragma warning restore SKEXP0010
				
				embeddings = await service.GenerateEmbeddingsAsync(recipesList);
				
				var documents = new List<SearchDocument>();
				for (int i = 0; i < recipes.Count; i++)
				{
					var recipe = recipes[i];
					//List<string> ingredients = [];
					//foreach (var ingredient in recipe.Ingredients)
					//{
					//	ingredients.Add(ingredient.Name);
					//}
					var document = new SearchDocument
					{
						{"Id",Guid.NewGuid().ToString()},
						{"Name",recipe.Name},
						{"Description",recipe.Description },
						//{"Ingredients",ingredients},
						{"Instructions",recipe.Instructions},
						{"IsVeg",recipe.IsVeg },
						{ "RecipeEmbeddings",embeddings[i].Span.ToArray()}
					};
					documents.Add(document);
				}
				Response<IndexDocumentsResult> res = await searchClient.UploadDocumentsAsync(documents);
				logger.LogInformation($"Embedded Recipes Uploaded, {res}");
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}
	}
}
