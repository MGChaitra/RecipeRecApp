using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RecipeAPIProcessor.Contacts;
using RecipeAPIProcessor.Services;

namespace RecipeAPIProcessor.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static void AddAzureApplicationServices(this IServiceCollection services)
        {
            // Register ConfigurationService
            services.AddSingleton<ConfigurationService>();

            services.AddSingleton<IAzureAISearchService, AzureAISearchService>(sp =>
            {
                var configService = sp.GetRequiredService<ConfigurationService>();
                var logger = sp.GetRequiredService<ILogger<AzureAISearchService>>();

                return new AzureAISearchService(
                    configService.GetAzureSearchServiceName(),
                    configService.GetAzureSearchApiKey(),
                    configService.GetAzureSearchEndpoint(),
                    logger
                );
            });

            // Register Cosmos Client using Configuration Service
            services.AddSingleton<CosmosClient>(sp =>
            {
                var configService = sp.GetRequiredService<ConfigurationService>();
                return new CosmosClient(configService.GetCosmosDbEndpoint(), configService.GetCosmosDbKey());
            });

            // Register CosmosDB Service
            services.AddSingleton<ICosmosDbService, CosmosDbService>(sp =>
            {
                var configService = sp.GetRequiredService<ConfigurationService>();
                var cosmosClient = sp.GetRequiredService<CosmosClient>();
                return new CosmosDbService(
                    cosmosClient,
                    configService.GetCosmosDbDatabaseName(),
                    configService.GetCosmosDbContainerName()
                );
            });

            // Upload Service
            services.AddHttpClient<IUploadToIndexService, UploadToIndexService>();
            services.AddSingleton<IUploadToIndexService>(sp =>
            {
                var configService = sp.GetRequiredService<ConfigurationService>();
                var httpClient = sp.GetRequiredService<HttpClient>();

                return new UploadToIndexService(httpClient, configService.GetAzureSearchUploadEndpoint(), configService.GetAzureSearchApiKey());
            });
        }
    }
}
