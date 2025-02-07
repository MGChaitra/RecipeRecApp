using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;
using RecipeRecWebApp.Services;

namespace RecipeRecWebApp.Components
{
    public partial class RecipeDisplay
    {
        private bool isLoading = false;
        private List<RecipeModel> recipeList = new();
        private bool showSummaryModal = false;
        private RecipeModel? selectedRecipe;
        private string? selectedRecipeSummary;
        private bool showWarning = false;
        private ElementReference recipeCardContainer;
        private async void HandleSearchClick()
        {
            if (SharedDataModel.SelectedIngredients.Count > 0)
            {
                showWarning = false;
                await ProcessRecipes();
            }
            else
            {
                showWarning = true;
            }
        }
       

        private async Task ScrollLeft()
        {

            await JS.InvokeAsync<object>("scrollRecipeCards", recipeCardContainer, -300);

        }

        private async Task ScrollRight()
        {
            await JS.InvokeAsync<object>("scrollRecipeCards", recipeCardContainer, 300);

        }
        private async Task ProcessRecipes()
        {
            if (SharedDataModel.SelectedIngredients.Count == 0)
            {
                logger.LogWarning("No ingredients selected.");
                return;
            }

            isLoading = true;
            RecipeState.ClearRecipes();
            StateHasChanged();

            var recipes = await RecipeSearchService.SearchRecipesAsync(SharedDataModel.SelectedIngredients.Select(i => i.Name).ToList());

            if (recipes.Count == 0)
            {
                logger.LogWarning("No recipes found in Azure Search. Requesting AI-generated recipes...");
                recipes = await RecipeSearchService.GenerateRecipesAsync(SharedDataModel.SelectedIngredients.Select(i => i.Name).ToList());
            }

            RecipeState.SetRecipes(recipes);

            isLoading = false;
            StateHasChanged();
        }

        private async Task GetRecipeSummary(RecipeModel recipe)
        {
            selectedRecipe = recipe;
            selectedRecipeSummary = null;
            showSummaryModal = true;

            selectedRecipeSummary = await RecipeSearchService.SummarizeRecipeAsync(recipe);

            StateHasChanged();
        }

        private void CloseModal()
        {
            showSummaryModal = false;
            selectedRecipe = null;
            selectedRecipeSummary = null;
        }

        private void AddToFavorites()
        {
            if (selectedRecipe != null)
            {
              
                if (!SharedDataModel.FavoriteRecipes.Any(r => r.recipe_name == selectedRecipe.recipe_name))
                {
                    SharedDataModel.FavoriteRecipes.Add(selectedRecipe);
                    StateHasChanged();
                    SharedDataModel.UpdateChanges();
                }
            }
        }
    }
}