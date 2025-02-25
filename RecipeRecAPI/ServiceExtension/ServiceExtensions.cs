using Configuration;
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
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Register ConfigurationService
            services.AddSingleton<ConfigurationService>();

         
            // Ingredient Service
            services.AddSingleton<IIngredientService, IngredientService>();

        }
    }
}
