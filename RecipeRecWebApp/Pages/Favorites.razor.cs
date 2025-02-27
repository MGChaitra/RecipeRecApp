using System.Net.Http.Json;
using Models;
using RecipeRecWebApp.Services;

namespace RecipeRecWebApp.Pages
{
    public partial class Favorites
    {
        private RecipeModel? selectedRecipe;
        private SummarizedRecipeModel? selectedRecipeSummary;
        private bool showSummaryModal = false;
        private List<FavoriteRecipeModel> favoriteRecipes = new();
        protected override async Task OnInitializedAsync()
        {
            favoriteRecipes = await CosmosDbService.GetAllFavRecipe();
            FavoriteStateService.SetFavorites(favoriteRecipes);
            StateHasChanged();
        }


        private async Task RemoveFromFavorites(FavoriteRecipeModel recipe)
        {
            bool success = await CosmosDbService.RemoveFromFv(recipe.id);
            if (success)
            {
                FavoriteStateService.RemoveFavorites(recipe.recipe_name);
                favoriteRecipes.Remove(recipe);
            }
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