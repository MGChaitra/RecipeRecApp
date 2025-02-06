using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;

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
        private void HandleSearchClick()
        {
            if (SharedDataModel.SelectedIngredients.Count > 0)
            {
                showWarning = false;
                ProcessRecipes();
            }
            else
            {
                showWarning = true;
            }
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

            string query = string.Join(", ", SharedDataModel.SelectedIngredients.Select(i => i.Name));

            try
            {
                var recipes = await Http.GetFromJsonAsync<List<RecipeModel>>($"api/RecipeSearch/search?query={query}") ?? new List<RecipeModel>();

                if (recipes.Count == 0)
                {
                    logger.LogWarning("No recipes found in Azure Search. Requesting AI-generated recipes...");
                    var response = await Http.PostAsJsonAsync("api/RecipeCustom/generate", SharedDataModel.SelectedIngredients.Select(i => i.Name).ToList());

                    if (response.IsSuccessStatusCode)
                    {
                        recipes = await response.Content.ReadFromJsonAsync<List<RecipeModel>>() ?? new List<RecipeModel>();
                    }
                }

                RecipeState.SetRecipes(recipes); 
            }
            catch (Exception ex)
            {
                logger.LogError($"Error fetching recipes: {ex.Message}");
            }

            isLoading = false;
            StateHasChanged();
        }
        

        private async Task ScrollLeft()
        {

            await JS.InvokeAsync<object>("scrollRecipeCards", recipeCardContainer, -300);

        }

        private async Task ScrollRight()
        {
            await JS.InvokeAsync<object>("scrollRecipeCards", recipeCardContainer, 300);

        }
        private async Task GetRecipeSummary(RecipeModel recipe)
        {
            selectedRecipe = recipe;
            selectedRecipeSummary = null;
            showSummaryModal = true;

            try
            {
                var response = await Http.PostAsJsonAsync("api/RecipeCustom/summarize", new List<RecipeModel> { recipe });

                if (response.IsSuccessStatusCode)
                {
                    var summaryList = await response.Content.ReadFromJsonAsync<List<SummarizedRecipeModel>>();
                    selectedRecipeSummary = summaryList?.FirstOrDefault()?.Summary;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error fetching summary: {ex.Message}");
            }

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
                SharedDataModel.FavoriteRecipes.Add(selectedRecipe);
                StateHasChanged();
                SharedDataModel.UpdateChanges();
            }
        }
    }
}