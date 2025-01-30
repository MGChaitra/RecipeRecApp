using RecipeRecAPI.Contracts;
using RecipeRecAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddScoped<IIngredientService,IngredientService>();

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
