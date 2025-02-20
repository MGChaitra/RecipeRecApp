using Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace RecipeRecWebApp.Pages
{
    public partial class Pantry
    {
   
     
        protected override void OnInitialized()
        {
            StateHasChanged();
        }
        public void HandleSearchClick()
        {
            Navigation.NavigateTo("/");
        }
        public void DeleteSelected(IngredientModel item)
        {
            try
            {
                logger.LogInformation("Deleting ingredient");
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
