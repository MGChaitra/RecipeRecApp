using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models;
using RecipeRecWebApp.Services;

namespace RecipeRecWebApp.Pages
{
    public partial class Recipes
    {
        

        private string searchTerm = string.Empty;
        private string selectedCategory = string.Empty;
        private string newIngredientName = string.Empty;
        private List<CategoryModel> FilteredCategories = new();
        private bool isLoading = false;
        private bool showWarning = false;
        private bool isAddIngredientVisible = false;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;

              
                var ingredients = await IngredientService.GetIngredientsAsync();

                foreach(var ingredient in ingredients)
                {
                    ingredient.Selected=false;  
                }
               
                SharedDataModel.Ingredients = ingredients;
                SharedDataModel.Categories = ingredients
                    .GroupBy(ingredient => ingredient.food_group, StringComparer.OrdinalIgnoreCase)
                    .Select(group => new CategoryModel
                    {
                        Name = group.Key,
                        Ingredients = group.ToList(),
                        IsExpanded = false
                    })
                    .ToList();

                FilteredCategories = SharedDataModel.Categories;
                isLoading = false;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error initializing data: {ex.Message}");
                isLoading = false;
            }
        }
        private void HandleSearchClick()
        {
            if (SharedDataModel.SelectedIngredients.Count > 0)
            {
                showWarning = false;
                 Navigation.NavigateTo("/Pantry");
            }
            else
            {
                showWarning = true;
            }
        }

        private void ToggleAddIngredientFields()
        {
            isAddIngredientVisible = !isAddIngredientVisible;
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
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex.Message}");
            }

        }
        private async Task AddIngredient()
        {
            if (string.IsNullOrWhiteSpace(selectedCategory) || string.IsNullOrWhiteSpace(newIngredientName))
            {
                logger.LogError("All fields must be filled.");
                return;
            }

            try
            {
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
                if (success)
                {
                    var category = SharedDataModel.Categories.FirstOrDefault(c => c.Name == selectedCategory);
                    category?.Ingredients.Add(newIngredient);
                    SharedDataModel.Ingredients.Add(newIngredient);
                    SharedDataModel.SelectedIngredients.Add(newIngredient);
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
        }
    }
}


