using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;

namespace RecipeRecWebApp.Components
{
	public partial class PopUpComponent
	{
		[Parameter] public bool display { get; set; }
		[Parameter] public EventCallback OnClose { get; set; }
		[Parameter] public RecipeModel recipeModel { get; set; }

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
				
				recipe.Instructions = await recipeService.CustomInstructions(recipeModel);

				if (recipe.IsFav)
				{
					var message = await Favorites.UpdateFavorites(recipe);
					await js.InvokeVoidAsync("alert", $"{message}");
				}

				loading = "";
				SharedDataModel.UpdateChanges();
				StateHasChanged();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}
	}
}