using Models;

namespace RecipeRecWebApp.Pages
{
	public partial class Favorites
	{
		private Dictionary<RecipeModel, bool> display = [];
		protected override void OnInitialized()
		{
			SharedDataModel.OnChanged += StateHasChanged;
			StateHasChanged();
		}

		public void Dispose()
		{
			SharedDataModel.OnChanged -= StateHasChanged;
		}

		public void DeleteSelected(RecipeModel item)
		{
			try
			{
				logger.LogInformation("Deleting data");
				item.IsFav = false;
				SharedDataModel.FavoriteRecipes.Remove(item);
				SharedDataModel.UpdateChanges();
				StateHasChanged();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

		}
		private void OpenRecipe(RecipeModel recipe)
		{
			display[recipe] = true;
		}

		private void CloseRecipe(RecipeModel recipe)
		{
			display[recipe] = false;
		}
	}
}