using Models;

namespace RecipeRecWebApp.Pages
{
	public partial class Favorites
	{
		private Dictionary<RecipeModel, bool> display = [];
		protected override void OnInitialized()
		{
			try
			{
				SharedDataModel.OnChanged += StateHasChanged;
				StateHasChanged();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		public void Dispose()
		{
			try
			{
				SharedDataModel.OnChanged -= StateHasChanged;
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
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
			try
			{
				display[recipe] = true;
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		private void CloseRecipe(RecipeModel recipe)
		{
			try
			{
				display[recipe] = false;
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}
	}
}