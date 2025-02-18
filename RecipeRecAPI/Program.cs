using Microsoft.SemanticKernel;
using RecipeRec.KernelOps.Contracts;
using RecipeRec.KernelOps.Plugins;
using RecipeRecAPI.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddCors(setUp =>
{
	setUp.AddPolicy("cors", setUp =>
	{
		setUp.AllowAnyHeader();
		setUp.AllowAnyMethod();
		setUp.AllowAnyOrigin();
	});
});
ServiceRegistrar.Register(builder.Services);

var app = builder.Build();
try
{
	var kernelProvider = app.Services.GetRequiredService<IKernalProvider>();
	var kernel = kernelProvider.CreateKernal();
	var settings = kernelProvider.RequiredSettings();
	var res = await kernel.InvokePromptAsync("create azure search recipe index", new(settings));
}catch(Exception ex)
{
	Console.WriteLine($"Error: {ex.Message}"); ;
}
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
