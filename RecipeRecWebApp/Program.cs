using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Models;
using RecipeRecWebApp;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(configuration.GetSection("ApiEndpoint").Value!) });
builder.Services.AddScoped<IIngredientService,IngredientService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();

var app = builder.Build();
try
{
	var ingredientService = app.Services.GetRequiredService<IIngredientService>();
	SharedDataModel.Ingredients = await ingredientService.GetIngredientsAsync();
	ingredientService.MapIngredients();

	var favoritesService = app.Services.GetRequiredService<IFavoritesService>();
	await favoritesService.GetAllFavorites();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

await app.RunAsync();
