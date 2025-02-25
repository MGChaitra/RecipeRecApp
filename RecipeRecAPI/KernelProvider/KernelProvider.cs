using Configuration;
using Microsoft.SemanticKernel;
using RecipeRecAPI.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeRecAPI.KernelProvider
{
    public static class KernelProvider
    {
        public static void AddSemanticKernel(this IServiceCollection services)
        {
          
            services.AddSingleton<RecipeCustomPlugin>();

            services.AddSingleton(sp =>
            {
                var configService = sp.GetRequiredService<ConfigurationService>();
                var logger = sp.GetRequiredService<ILogger<RecipeCustomPlugin>>();
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddAzureOpenAIChatCompletion(
                    deploymentName: configService.GetAzureOpenAIDeploymentName(),
                    endpoint: configService.GetAzureOpenAIEndpoint(),
                    apiKey: configService.GetAzureOpenAIApiKey()
                );

                var kernel = kernelBuilder.Build();

                var recipePlugin = new RecipeCustomPlugin(configService, logger);
              
                kernel.Plugins.AddFromObject(recipePlugin, nameof(RecipeCustomPlugin));
                return kernel;
            });
        }
       
    }
}