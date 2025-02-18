using Microsoft.JSInterop;
using Models;

namespace RecipeRecWebApp.Components.Recipes
{
	public partial class RecipesComponent
	{

		private Dictionary<RecipeModel, bool> display = [];

		protected override void OnInitialized()
		{
			SharedDataModel.OnChanged += StateHasChanged;

		}

		public void Dispose()
		{
			SharedDataModel.OnChanged -= StateHasChanged;
		}

		private void ViewFavorites()
		{
			SharedDataModel.displayRecipe = false;
			SharedDataModel.UpdateChanges();
		}

		private void ToggleRecipe(RecipeModel recipe)
		{
			foreach(var item in display)
			{
				if (item.Key != recipe)
				{
					display[item.Key] = false;
				}
			}

			if (display.ContainsKey(recipe))
			{
				display[recipe] = !display[recipe];
			}
			else
			{
				display[recipe] = true;
			}
		}

		private void CloseRecipe(RecipeModel recipe)
		{
			display[recipe] = false;
		}
		private async void AddFavorite(RecipeModel recipe)
		{
			try
			{
				
				var message = await Favorites.SaveFavorite(recipe);
				await jsRuntime.InvokeVoidAsync("alert", $"{message}");

				recipe.IsFav = true;
				SharedDataModel.FavoriteRecipes.Add(recipe);
				SharedDataModel.UpdateChanges();
				StateHasChanged();
			}
			catch(Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		private async Task RemoveFavorite(RecipeModel recipe)
		{
			try
			{
				if (SharedDataModel.FavoriteRecipes.Contains(recipe))
				{
					var message = await Favorites.RemoveFavorite(recipe);
					await jsRuntime.InvokeVoidAsync("alert", $"{message}");
					
					recipe.IsFav = false;
					SharedDataModel.FavoriteRecipes.Remove(recipe);
					SharedDataModel.UpdateChanges();
					StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}
	}
}