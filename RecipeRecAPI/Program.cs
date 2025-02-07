using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using RecipeRecAPI.Contracts;
using RecipeRecAPI.Plugins;
using RecipeRecAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(setUp =>
{
    setUp.AddPolicy("cors", setUp =>
    {
        setUp.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials();
    });
});
// Add services to the container.

IConfiguration configuration = new ConfigurationBuilder()

.AddJsonFile("appsettings.json")
.AddUserSecrets<Program>()

.Build();
    var searchServiceName = configuration["Azure:Search:ServiceName"];
    var searchApiKey = configuration["Azure:Search:ApiKey"];
    var endpoint = configuration["Azure:Search:Endpoint"];

if (string.IsNullOrWhiteSpace(searchServiceName))
{
    throw new ArgumentNullException(nameof(searchServiceName), "Search service name cannot be null or empty.");
}

if (string.IsNullOrWhiteSpace(searchApiKey))
{
    throw new ArgumentNullException(nameof(searchApiKey), "Search API key cannot be null or empty.");
}

if (string.IsNullOrWhiteSpace(endpoint))
{
    throw new ArgumentNullException(nameof(endpoint), "Endpoint cannot be null or empty.");
}
builder.Services.AddSingleton<AzureAISearchService>(serviceProvider =>
{


    var logger = serviceProvider.GetRequiredService<ILogger<AzureAISearchService>>();
    return new AzureAISearchService(searchServiceName, searchApiKey, endpoint, logger);
});


#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var searchService = builder.Services.BuildServiceProvider().GetRequiredService<AzureAISearchService>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
await searchService.CreateIndexAsync();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: configuration["AzureOpenAI:DeploymentName"]!,
        endpoint: configuration["AzureOpenAI:Endpoint"]!,
        apiKey: configuration["AzureOpenAI:ApiKey"]!
    );

    var kernel = kernelBuilder.Build();

    var logger = sp.GetRequiredService<ILogger<RecipeCustomPlugin>>();
    var recipePlugin = new RecipeCustomPlugin(configuration, logger);

    kernel.Plugins.AddFromObject(recipePlugin, nameof(RecipeCustomPlugin));

    return kernel;
});
builder.Services.AddSingleton<RecipeCustomPlugin>();



builder.Services.AddSingleton<IIngredientService, IngredientService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
