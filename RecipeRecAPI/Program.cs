using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using RecipeAPIProcessor.Contacts;
using RecipeAPIProcessor.Services;
using RecipeRecAPI;
using RecipeRecAPI.Contracts;
using RecipeRecAPI.Plugins;
using RecipeRecAPI.ServiceExtension;
using RecipeRecAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(setUp =>
{
    setUp.AddPolicy("cors", setUp =>
    {
        setUp.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .WithOrigins("https://localhost:7028/");
    });
});
// Add services to the container.
// Load configuration
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

// Add services via extension methods
builder.Services.AddApplicationServices(configuration);
builder.Services.AddSemanticKernel(configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
