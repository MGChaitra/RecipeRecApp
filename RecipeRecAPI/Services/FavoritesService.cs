using Models;
using Microsoft.Azure.Cosmos;
using System.Text.Json;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Services
{
	public class FavoritesService(ILogger<FavoritesService> logger, IConfiguration configuration) : IFavoritesService
	{
		private readonly ILogger<FavoritesService> logger = logger;
		private readonly string ConnectionString = configuration["CosmosDBSettings:ConnectionString"]!;
		private readonly string Database = configuration["CosmosDBSettings:Database"]!;
		private readonly string Container = configuration["CosmosDBSettings:Container"]!;

		private Container FetchContainer()
		{
			CosmosClient DbClient = new CosmosClient(ConnectionString, new CosmosClientOptions
			{
				Serializer = new SystemTextJsonSerializer()
			});
			var container = DbClient.GetContainer(Database, Container);
			return container;
		}

		public async Task<string> SaveFavorites(RecipeModel recipe)
		{
			try
			{
				var container = FetchContainer();
				await container.CreateItemAsync(recipe, new PartitionKey(recipe.Id));
				return "Saved To Favorites";
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return "Internal Server Error: Unable to save recipe";
			}
		}

		public async Task<string> DeleteFavorites(string id)
		{
			try
			{
				var container = FetchContainer();
				await container.DeleteItemAsync<dynamic>(id, new PartitionKey(id));
				return "Removed from favorites";
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return "Internal Server Error: Unable to unsave recipe";
			}
		}

		public async Task<string> UpdateFavorites(RecipeModel recipe)
		{
			try
			{
				var container = FetchContainer();
				var response = await container.UpsertItemAsync(recipe, new PartitionKey(recipe.Id));
				return "Custom Instructions Updated";
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return "Internal Server Error: Unable to update recipe";
			}
		}

		public async Task<List<RecipeModel>> GetAllRecipes()
		{
			List<RecipeModel> fav_recipes = [];
			try
			{
				var container = FetchContainer();
				var queryRes = container.GetItemQueryIterator<RecipeModel>("SELECT * FROM c");

				while (queryRes.HasMoreResults)
				{
					var response = await queryRes.ReadNextAsync();
					fav_recipes.AddRange(response);
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

			return fav_recipes;
		}
	}

	class SystemTextJsonSerializer : CosmosSerializer
	{
		private readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		public override T FromStream<T>(Stream stream)
		{
			using (stream)
			{
				return JsonSerializer.Deserialize<T>(stream, _options)!;
			}
		}

		public override Stream ToStream<T>(T input)
		{
			MemoryStream stream = new MemoryStream();
			using (Utf8JsonWriter writer = new Utf8JsonWriter(stream))
			{
				JsonSerializer.Serialize(writer, input, _options);
			}
			stream.Position = 0;
			return stream;
		}
	}
}
