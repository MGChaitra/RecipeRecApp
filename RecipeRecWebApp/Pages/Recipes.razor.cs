using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models;

namespace RecipeRecWebApp.Pages
{
	public partial class Recipes
	{
		private string searchTerm = string.Empty;
		private List<CategoryModel> FilteredCategories = [];
		private string selectMessage = "";
		private string selectedCategory = string.Empty;
		private string newIngredientName = string.Empty;
		private bool isAddIngredientVisible = false;
		private bool isLoading = false;
		private bool isLoadingRecipe = false;


		protected override void OnInitialized()
		{
			try
			{
				FilteredCategories = SharedDataModel.Categories;
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}
		public void ToggleAddIngredientFields()
		{
			isAddIngredientVisible = !isAddIngredientVisible;
		}

		public async Task HandleKeyDown(KeyboardEventArgs e)
		{
			try
			{
				if (e.Key == "Enter")
				{
					await FilterIngredients();
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

		private async Task FilterIngredients()
		{
			try
			{
				var flag = 0;
				foreach (var category in FilteredCategories)
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
			try
			{
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

		public void ToggleIngredientSelection(IngredientModel ingredient)
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
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}

		}
		public async Task AddIngredient()
		{
			isLoading = true;

			try
			{
				if (string.IsNullOrWhiteSpace(selectedCategory) || string.IsNullOrWhiteSpace(newIngredientName))
				{
					logger.LogError("All fields must be filled.");
					isLoading = false;
					return;
				}

				var newId = SharedDataModel.Ingredients.Any()
					? SharedDataModel.Ingredients.Max(i => i.Id) + 1
					: 1;

				var newIngredient = new IngredientModel
				{
					Id = newId,
					Name = newIngredientName,
					food_group = selectedCategory,
					Selected = true,
					Visible = true,
				};


				var success = await IngredientService.AddIngredientAsync(newIngredient);
				var ingredients = await IngredientService.GetIngredientsAsync();
				if (success)
				{
					if (SharedDataModel.Ingredients.Count < ingredients.Count)
					{
						SharedDataModel.Ingredients.Add(newIngredient);
						SharedDataModel.SelectedIngredients.Add(newIngredient);
						IngredientService.MapIngredients();
					}
					else
					{
						foreach (var item in SharedDataModel.Ingredients)
						{
							if (String.Equals(item.Name, newIngredient.Name, StringComparison.OrdinalIgnoreCase))
							{
								item.Visible = true;
								item.Selected = true;
								if(!SharedDataModel.SelectedIngredients.Contains(item)) SharedDataModel.SelectedIngredients.Add(item);
							}
						}
					}

					FilteredCategories = SharedDataModel.Categories;
					ToggleCategory(newIngredient.food_group);
					await JSRuntime.InvokeVoidAsync("scrollToClass", "ingredient-card");
					SharedDataModel.UpdateChanges();

					logger.LogInformation("Ingredient added successfully.");
					newIngredientName = string.Empty;
					selectedCategory = string.Empty;
				}
				else
				{
					logger.LogError("Failed to add ingredient.");
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error adding ingredient: {ex.Message}");
			}
			isLoading=false;
		}

		public async Task GetRecipes()
		{
			logger.LogInformation("Fetching Recipes...");
			isLoadingRecipe = true;
			try
			{
				if (SharedDataModel.SelectedIngredients.Count > 0)
				{
					SharedDataModel.Recipes = await IngredientService.GetRecipes(SharedDataModel.SelectedIngredients);
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


