using Microsoft.AspNetCore.Components;
using Models;

namespace RecipeRecWebApp.Components
{
	public partial class PopUpComponent
	{
		[Parameter] public bool display { get; set; }
		[Parameter] public EventCallback OnClose { get; set; }
		[Parameter] public RecipeModel recipeModel { get; set; }
		[Parameter] public List<IngredientModel> Ingredients { get; set; }

		private string loading = "";

		private async Task CloseModel()
		{
			await OnClose.InvokeAsync();
		}
		public async Task CustomizeInstructions(RecipeModel recipe)
		{
			try
			{
				loading = "Loading...";
				StateHasChanged();
				recipe.Instructions = await ingredientService.CustomInstructions(recipeModel);
				loading = "";
				SharedDataModel.UpdateChanges();
				StateHasChanged();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}