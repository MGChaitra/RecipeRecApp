using Microsoft.AspNetCore.Components.Web;
using Models;

namespace RecipeRecWebApp.Components
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

		private void OpenRecipe(RecipeModel recipe)
		{
			display[recipe] = true;
		}

		private void CloseRecipe(RecipeModel recipe)
		{
			display[recipe] = false;
		}

		private void AddFavorite(RecipeModel recipe)
		{
			recipe.IsFav = true;
			SharedDataModel.FavoriteRecipes.Add(recipe);
			SharedDataModel.UpdateChanges();
			StateHasChanged();
		}

		private void RemoveFavorite(RecipeModel recipe)
		{
			recipe.IsFav = false;
			if (SharedDataModel.FavoriteRecipes.Contains(recipe)) 
			{ 
				SharedDataModel.FavoriteRecipes.Remove(recipe);
				SharedDataModel.UpdateChanges();
				StateHasChanged();
			}
		}
	}
}