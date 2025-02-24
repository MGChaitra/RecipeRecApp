using Microsoft.Azure.Cosmos;
using RecipeAPIProcessor.Contacts;
using RecipeAPIProcessor.Services;
using RecipeRecAPI.Contracts;
using RecipeRecAPI.Plugins;
using RecipeRecAPI.Services;

namespace RecipeRecAPI.ServiceExtension
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            // Azure AI Search Service
            var searchServiceName = configuration["Azure:Search:ServiceName"];
            var searchApiKey = configuration["Azure:Search:ApiKey"];
            var endpoint = configuration["Azure:Search:Endpoint"];

            if (string.IsNullOrWhiteSpace(searchServiceName) || string.IsNullOrWhiteSpace(searchApiKey) || string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException("Azure Search configurations cannot be null or empty.");
            }

            services.AddSingleton<IAzureAISearchService, AzureAISearchService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AzureAISearchService>>();
                return new AzureAISearchService(searchServiceName, searchApiKey, endpoint, logger);
            });

            //Cosmos Service

            // Cosmos DB Client
            services.AddSingleton<CosmosClient>(sp => new CosmosClient(
                configuration["CosmosDb:Endpoint"]!,
                configuration["CosmosDb:Key"]!
            ));

            // Cosmos DB Service
            services.AddSingleton<ICosmosDbService, CosmosDbService>(sp => new CosmosDbService(
                sp.GetRequiredService<CosmosClient>(),
                configuration["CosmosDb:DatabaseName"]!,
                configuration["CosmosDb:ContainerName"]!
            ));

            // Recipe Custom Plugin
            services.AddSingleton<RecipeCustomPlugin>();

            // Ingredient Service
            services.AddSingleton<IIngredientService, IngredientService>();

            // Upload Service
            services.AddHttpClient<IUploadToIndexService, UploadToIndexService>();
            services.AddSingleton<IUploadToIndexService>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var searchApiKey = configuration["Azure:Search:ApiKey"]!;
                var uploadEndpoint = configuration["Azure:Search:UploadEndpoint"]!;

                return new UploadToIndexService(httpClient, uploadEndpoint, searchApiKey);
            });

            
        }
    }
}
