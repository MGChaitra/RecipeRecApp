using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models;

namespace RecipeRecWebApp.Pages
{

	public partial class MainPage
	{
		protected override void OnInitialized()
		{
			SharedDataModel.OnChanged += StateHasChanged;

		}

		public void Dispose()
		{
			SharedDataModel.OnChanged -= StateHasChanged;
		}

	}
}


