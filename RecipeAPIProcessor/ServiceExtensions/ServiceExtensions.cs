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

            //Register AzureSearchService
            services.AddSingleton<IAzureAISearchService, AzureAISearchService>();


            // Register CosmosDB Service
            services.AddSingleton<ICosmosDbService, CosmosDbService>();

            // Upload Service
            services.AddHttpClient<IUploadToIndexService, UploadToIndexService>();

          
           
        }
    }
}
