using RecipeRec.AzureAiSearch.IndexCreation.Contracts;
using RecipeRec.AzureAiSearch.IndexCreation.Services;
using RecipeRec.KernelOps.Contracts;
using RecipeRec.KernelOps.KernelProvider;
using RecipeRec.KernelOps.Plugins;
using RecipeRecAPI.Contracts;
using RecipeRecAPI.Services;

namespace RecipeRecAPI.Helper
{
	public class ServiceRegistrar
	{
		public static void Register(IServiceCollection services)
		{
			try
			{
				services.AddSingleton<IIngredientService, IngredientService>();
				services.AddSingleton<IRecipeServices, RecipeServices>();
				services.AddSingleton<IKernalProvider,KernalProvider>();
				services.AddSingleton<IFavoritesService, FavoritesService>();
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
