using System.Collections.Concurrent;
using System.Drawing;
using System.Text;
using Models;
using Microsoft.Azure.Cosmos;
using RecipeAPIProcessor.Contacts;
using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RecipeAPIProcessor.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;
        private readonly CosmosClient _client;
        private readonly string _connectionString;
        private readonly string _database;
        private readonly string _endpoint;
        private readonly string _containerName;
        private readonly ConfigurationService _configurationService;
        private readonly ILogger<ICosmosDbService> _logger;

        public CosmosDbService(ConfigurationService configuration, ILogger<ICosmosDbService> logger)
        {
            _configurationService = configuration;
            _connectionString = configuration.GetCosmosDbEndpoint();
            _database = configuration.GetCosmosDbDatabaseName();
            _containerName = configuration.GetCosmosDbContainerName();
            _endpoint = configuration.GetCosmosDbKey();
            _client = new CosmosClient(_connectionString, _endpoint);
            _container = _client.GetContainer(_database, _containerName);
            _logger = logger;
        }

        public async Task AddRecipeAsync(FavoriteRecipeModel recipe)
        {
            try
            {
                _logger.LogInformation("Adding recipe with id {RecipeId}", recipe.id);
                await _container.CreateItemAsync(recipe, new PartitionKey(recipe.id));
                _logger.LogInformation("Recipe with id {RecipeId} added successfully", recipe.id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding recipe with id {RecipeId}", recipe.id);
                throw;
            }
        }

        public async Task<List<FavoriteRecipeModel>> GetRecipesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all recipes");
                var query = _container.GetItemQueryIterator<FavoriteRecipeModel>("SELECT * FROM c");
                List<FavoriteRecipeModel> results = new List<FavoriteRecipeModel>();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ReadNextAsync());
                }

                _logger.LogInformation("Fetched {Count} recipes", results.Count);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipes");
                throw;
            }
        }

        public async Task DeleteRecipeAsync(string id)
        {
            try
            {
                _logger.LogInformation("Deleting recipe with id {RecipeId}", id);
                var recipe = await _container.ReadItemAsync<FavoriteRecipeModel>(id, new PartitionKey(id));
                await _container.DeleteItemAsync<FavoriteRecipeModel>(id, new PartitionKey(id));
                _logger.LogInformation("Recipe with id {RecipeId} deleted successfully", id);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Recipe with id {RecipeId} not found", id);
                throw new Exception($"Recipe with id {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe with id {RecipeId}", id);
                throw;
            }
        }

        public async Task<FavoriteRecipeModel> GetRecipeByNameAsync(string recipeName)
        {
            try
            {
                _logger.LogInformation("Fetching recipe with name {RecipeName}", recipeName);
                var query = _container.GetItemQueryIterator<FavoriteRecipeModel>(
                    new QueryDefinition("SELECT * FROM c WHERE c.recipe_name = @recipeName")
                    .WithParameter("@recipeName", recipeName));

                var results = await query.ReadNextAsync();
                var recipe = results.FirstOrDefault();

                if (recipe != null)
                {
                    _logger.LogInformation("Recipe with name {RecipeName} fetched successfully", recipeName);
                    return recipe;
                }
                else
                {
                    _logger.LogWarning("Recipe with name {RecipeName} not found", recipeName);
                    throw new Exception($"Recipe with name {recipeName} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe with name {RecipeName}", recipeName);
                throw;
            }
        }


    }
}
