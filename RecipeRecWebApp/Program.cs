using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using RecipeRecWebApp;
using RecipeRecWebApp.Contracts;
using RecipeRecWebApp.ServiceExtension;
using RecipeRecWebApp.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddApplicationService();

var app = builder.Build();
await app.RunAsync();
