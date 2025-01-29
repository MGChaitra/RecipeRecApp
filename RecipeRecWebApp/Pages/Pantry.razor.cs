using Models;

namespace RecipeRecWebApp.Pages
{
	public partial class Pantry
	{

		protected override void OnInitialized()
		{
			StateHasChanged();
		}

		public void DeleteSelected(IngredientModel item)
		{
			try
			{
				logger.LogInformation("Deleting data");
				SharedDataModel.SelectedIngredients.Remove(item);
				item.Selected = false;
				StateHasChanged();
				SharedDataModel.UpdateChanges();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

		}
	}
}