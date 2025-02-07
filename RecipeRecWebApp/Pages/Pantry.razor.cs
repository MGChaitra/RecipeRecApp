using Models;
using RecipeRecWebApp.Services;

namespace RecipeRecWebApp.Pages
{
	public partial class Pantry
	{

		private string selectMessage = "";
		private bool isLoadingRecipe = false;
		protected override void OnInitialized()
		{
			try
			{
				StateHasChanged();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		public void DeleteSelected(IngredientModel item)
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
		public async Task GetRecipes()
		{
			logger.LogInformation("Fetching Recipes...");
			isLoadingRecipe = true;
			try
			{
				if (SharedDataModel.SelectedIngredients.Count > 0)
				{
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