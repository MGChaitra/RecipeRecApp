using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecipeRecWebApp;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7175/") });
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddSingleton<RecipeStateService>();
var app = builder.Build();

await app.RunAsync();
