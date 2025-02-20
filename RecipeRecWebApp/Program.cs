using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using RecipeRecWebApp;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Services;
using System.Net.Http;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7175/") });
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddSingleton<IRecipeStateService, RecipeStateService>();
builder.Services.AddHttpClient<IRecipeSearchService, RecipeSearchService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7175/"); // Set API base URL
});
builder.Services.AddHttpClient<ICosmosDbService, CosmosDbService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7175/"); // Set API base URL
});
builder.Services.AddSingleton<IFavoriteStateService,FavoriteStateService>();
var app = builder.Build();
await app.RunAsync();
