using Microsoft.SemanticKernel;
using RecipeRecAPI.Plugins;

namespace RecipeRecAPI
{
    public static class KernelProvider
    {
        public static void AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddAzureOpenAIChatCompletion(
                    deploymentName: configuration["AzureOpenAI:DeploymentName"]!,
                    endpoint: configuration["AzureOpenAI:Endpoint"]!,
                    apiKey: configuration["AzureOpenAI:ApiKey"]!
                );

                var kernel = kernelBuilder.Build();
                var logger = sp.GetRequiredService<ILogger<RecipeCustomPlugin>>();
                var recipePlugin = new RecipeCustomPlugin(configuration, logger);

                kernel.Plugins.AddFromObject(recipePlugin, nameof(RecipeCustomPlugin));

                return kernel;
            });
        }
    }
}