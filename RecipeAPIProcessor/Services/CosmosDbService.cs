
using System.Collections.Concurrent;
using System.Drawing;
using System.Text;
using Models;
using Microsoft.Azure.Cosmos;
using RecipeAPIProcessor.Contacts;
namespace RecipeAPIProcessor.Services
{
    public class CosmosDbService:ICosmosDbService

    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {

            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddRecipeAsync(FavoriteRecipeModel recipe)
        {
            await _container.CreateItemAsync(recipe, new PartitionKey(recipe.recipe_name));
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

        public async Task DeleteRecipeAsync(string recipeName)
        {
            try
            {
              
                var recipe = await _container.ReadItemAsync<FavoriteRecipeModel>(recipeName, new PartitionKey(recipeName));

              
                await _container.DeleteItemAsync<FavoriteRecipeModel>(recipeName, new PartitionKey(recipeName));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
               
                throw new Exception($"Recipe with id {recipeName} not found.");
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
