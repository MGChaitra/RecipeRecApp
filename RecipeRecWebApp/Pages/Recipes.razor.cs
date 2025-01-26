using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RecipeRecWebApp.Models;

namespace RecipeRecWebApp.Pages
{
    public partial class Recipes
    {
        private string searchTerm = string.Empty;
        private List<CategoryModel> FilteredCategories = [];
        protected override void OnInitialized()
        {
            try
            { 
                FilteredCategories = SharedDataModel.Categories;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error loading categories: {ex.Message}");
            }
        }
        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await FilterIngredients();
            }
        }
        private async Task FilterIngredients()
        {
            try
            {

				var flag = 0;
				foreach (var category in SharedDataModel.Categories)
				{
					foreach (var ingredient in category.Ingredients)
					{
						ingredient.Visible = (ingredient.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || searchTerm.Contains(ingredient.Name, StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrEmpty(searchTerm);

						if (ingredient.Visible)
						{
							category.IsExpanded = true;
						}
					}
					if (category.IsExpanded)
					{
						flag = 1;
					}
				}
				if (flag == 0)
				{
					await JSRuntime.InvokeVoidAsync("scrollToClass", "grid-container");
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("scrollToClass", "ingredient-card");
				}
			}
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }
		}

        private void ToggleCategory(string categoryName)
        {
            try {
				var category = SharedDataModel.Categories.FirstOrDefault(c => c.Name == categoryName);
				if (category != null)
				{
					category.IsExpanded = !category.IsExpanded;
					if (category.IsExpanded)
					{
						foreach (var ingredient in category.Ingredients)
						{
							ingredient.Visible = true;
						}
					}
				}
			}
            catch (Exception ex) 
            {
                logger.LogError($"Error: {ex.Message}");
            }
        }

        private void ToggleIngredientSelection(IngredientModel ingredient)
        {
            try
            {
				ingredient.Selected = !ingredient.Selected;
				if (ingredient.Selected)
				{
					SharedDataModel.SelectedIngredients.Add(ingredient);
				}
				else
				{
					SharedDataModel.SelectedIngredients.Remove(ingredient);
				}
				SharedDataModel.UpdateChanges();

			}
			catch(Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }
            
        }

    }
}