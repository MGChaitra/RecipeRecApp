
using System.Collections.Concurrent;
using System.Drawing;
using System.Text;
using Models;
using Microsoft.Azure.Cosmos;
using RecipeAPIProcessor.Contacts;
using Configuration;
using Microsoft.Extensions.Configuration;
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

        public CosmosDbService(ConfigurationService configuration)
        {
            _configurationService = configuration;
            _connectionString = configuration.GetCosmosDbEndpoint();
            _database = configuration.GetCosmosDbDatabaseName();
            _containerName = configuration.GetCosmosDbContainerName();
            _endpoint = configuration.GetCosmosDbKey();
            _client = new CosmosClient(_connectionString, _endpoint);
            _container = _client.GetContainer(_database, _containerName);
        }


        public async Task AddRecipeAsync(FavoriteRecipeModel recipe)
        {
            await _container.CreateItemAsync(recipe, new PartitionKey(recipe.id));
        }

        public async Task<List<FavoriteRecipeModel>> GetRecipesAsync()
        {
            var query = _container.GetItemQueryIterator<FavoriteRecipeModel>("SELECT * FROM c");
            List<FavoriteRecipeModel> results = new List<FavoriteRecipeModel>();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ReadNextAsync());
            }
            return results;
        }

        public async Task DeleteRecipeAsync(string id)
        {
            try
            {

                var recipe = await _container.ReadItemAsync<FavoriteRecipeModel>(id, new PartitionKey(id));


                await _container.DeleteItemAsync<FavoriteRecipeModel>(id, new PartitionKey(id));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                throw new Exception($"Recipe with id {id} not found.");
            }
        }
        public async Task<FavoriteRecipeModel> GetRecipeByNameAsync(string recipeName)
        {
            var query = _container.GetItemQueryIterator<FavoriteRecipeModel>(
                new QueryDefinition("SELECT * FROM c WHERE c.recipe_name = @recipeName")
                .WithParameter("@recipeName", recipeName));

            var results = await query.ReadNextAsync();
            return results.FirstOrDefault();
        }


    }
}
