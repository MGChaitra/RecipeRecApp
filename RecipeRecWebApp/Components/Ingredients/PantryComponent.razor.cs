using Models;

namespace RecipeRecWebApp.Components.Ingredients
{
	public partial class PantryComponent
	{
		private string selectMessage = "";
		private bool isLoadingRecipe = false;
		protected override void OnInitialized()
		{
			try
			{
				SharedDataModel.OnChanged += StateHasChanged;
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

		private void SelectIngredients()
		{
			SharedDataModel.selectIngredients = true;
			SharedDataModel.UpdateChanges();
		}
		private void DeleteSelected(IngredientModel item)
		{
			try
			{
				logger.LogInformation("Deleting data");
				item.Selected = false;
				SharedDataModel.SelectedIngredients.Remove(item);
				StateHasChanged();
				SharedDataModel.UpdateChanges();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

		}
		private async Task GetRecipes()
		{
			logger.LogInformation("Fetching Recipes...");
			isLoadingRecipe = true;
			try
			{
				if (SharedDataModel.SelectedIngredients.Count > 0)
				{
					SharedDataModel.displayRecipe = true;
					SharedDataModel.Recipes = await RecipeService.GetRecipes(SharedDataModel.SelectedIngredients);
					SharedDataModel.UpdateChanges();
				}
				else
				{
					selectMessage = "Select ingredients to proceed.";
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			isLoadingRecipe = false;
		}

	}
}