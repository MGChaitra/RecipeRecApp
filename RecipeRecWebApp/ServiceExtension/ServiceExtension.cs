using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Services;

namespace RecipeRecWebApp.ServiceExtension
{
    public static class ServiceExtension
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            //HTTP Client for API calls
            services.AddScoped(sp=> new HttpClient {BaseAddress= new Uri("https://localhost:7175/") });

            services.AddScoped<IIngredientService, IngredientService>();
            services.AddSingleton<IRecipeStateService, RecipeStateService>();
            services.AddSingleton<IFavoriteStateService, FavoriteStateService>();

            //HTTP Clients for API Communication
            services.AddHttpClient<IRecipeSearchService, RecipeSearchService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7175/"); // Set API base URL
            });
            services.AddHttpClient<ICosmosDbService, CosmosDbService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7175/"); // Set API base URL
            });
        
       
        }
    }
}
