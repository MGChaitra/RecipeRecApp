using Models;

namespace RecipeRecWebApp.Layout
{
    public partial class NavMenu
    {
        protected override void OnInitialized()
        {
            SharedDataModel.OnChanged += StateHasChanged;

        }
        public void Dispose()
        {
            SharedDataModel.OnChanged -= StateHasChanged;
        }

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}