using System.Net.Http.Json;
using Models;
using RecipeRecWebApp.Services;

namespace RecipeRecWebApp.Pages
{
    public partial class Favorites
    {
        private RecipeModel? selectedRecipe;
        private string? selectedRecipeSummary;
        private bool showSummaryModal = false;
        protected override void OnInitialized()
        {
            StateHasChanged();
        }

        private void RemoveFromFavorites(RecipeModel recipe)
        {

            SharedDataModel.FavoriteRecipes.Remove(recipe);
            StateHasChanged();
            SharedDataModel.UpdateChanges();
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
    }
}