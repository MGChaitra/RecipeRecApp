using Models;
using Microsoft.Azure.Cosmos;
using System.Text.Json;
using RecipeRecAPI.Contracts;

namespace RecipeRecAPI.Services
{
	public class FavoritesService : IFavoritesService
	{
		private readonly ILogger<FavoritesService> logger;
		private readonly string ConnectionString;
		private readonly string Database;
		private readonly string Container;
		private readonly CosmosClient DbContext;
		public FavoritesService(ILogger<FavoritesService> logger, IConfiguration configuration)
		{
			this.logger = logger;
			ConnectionString = configuration["CosmosDBSettings:ConnectionString"]!;
			Database = configuration["CosmosDBSettings:Database"]!;
			Container = configuration["CosmosDBSettings:Container"]!;
			DbContext = new CosmosClient(ConnectionString, new CosmosClientOptions
			{
				Serializer = new SystemTextJsonSerializer()
			});
		}

		/// <summary>
		/// Creates recipe record in Cosmos DB container, where recipe Id is partition key.
		/// </summary>
		/// <param name="recipe">Recipe record to be stored</param>
		/// <returns>string - Success or failure message.</returns>
		public async Task<string> SaveFavorites(RecipeModel recipe)
		{
			try
			{
				recipe.Id = Guid.NewGuid().ToString();
				var container = DbContext.GetContainer(Database,Container);
				await container.CreateItemAsync(recipe, new PartitionKey(recipe.Id));
				return "Saved To Favorites";
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return "Internal Server Error: Unable to save recipe";
			}
		}

		/// <summary>
		///	Deletes recipe record in Cosmos DB container, usingid, partion key..
		/// </summary>
		/// <param name="id">string - id to refer to the record to be deleted</param>
		/// <returns>string - Success or failure message.</returns>
		public async Task<string> DeleteFavorites(string id)
		{
			try
			{
				var container = DbContext.GetContainer(Database,Container);
				await container.DeleteItemAsync<dynamic>(id, new PartitionKey(id));
				return "Removed from favorites";
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return "Internal Server Error: Unable to unsave recipe";
			}
		}

		/// <summary>
		/// Updating/Replacing the recipes in Cosmos DB container, using partition key.
		/// </summary>
		/// <param name="recipe">New Recipe record to be replaced with.</param>
		/// <returns>string - Success or failure message.</returns>
		public async Task<string> UpdateFavorites(RecipeModel recipe)
		{
			try
			{
				var container = DbContext.GetContainer(Database, Container);
				var response = await container.UpsertItemAsync(recipe, new PartitionKey(recipe.Id));
				return "Custom Instructions Updated";
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return "Internal Server Error: Unable to update recipe";
			}
		}

		/// <summary>
		/// Fetching all saved recipes from Cosmos DB container.
		/// </summary>
		/// <returns>List of RecipeModel - favorite recipes from Cosmos DB container</returns>
		public async Task<List<RecipeModel>> GetAllRecipes()
		{
			List<RecipeModel> fav_recipes = [];
			try
			{
				var container = DbContext.GetContainer(Database, Container);
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

	/// <summary>
	/// Implements CosmosSerializer abstract class to Serialize the Model attributes to CosmosDB container attributes, ignoring case sensitivity for mapping fields appropriately.
	/// </summary>
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
