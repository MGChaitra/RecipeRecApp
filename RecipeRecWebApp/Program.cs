using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecipeRecWebApp;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IStartUpService,StartUpService>();
var app = builder.Build();
try
{
    var startUpService = app.Services.GetRequiredService<IStartUpService>();
    await startUpService.InitializeIngredientsAsync();
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}
await app.RunAsync();
