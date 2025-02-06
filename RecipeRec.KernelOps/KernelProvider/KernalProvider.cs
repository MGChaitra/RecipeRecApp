using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using RecipeRec.KernelOps.Contracts;
using RecipeRec.KernelOps.Helper;
using RecipeRec.KernelOps.Plugins;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RecipeRec.KernelOps.KernelProvider
{
	public class KernalProvider : IKernalProvider
	{
		public Kernel CreateKernal()
		{
			IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
			try
			{
				//configuration
				var configuration = ConfigurationHelper.Configuration;

				//Adding AI Models
				string? DeploymentName = configuration["AzureAiService:model"], 
						Endpoint = configuration["AzureAiService:endpoint"],
						ApiKey = configuration["AzureAiService:key"];
				
				
				kernelBuilder.AddAzureOpenAIChatCompletion(
					DeploymentName!,
					Endpoint!,
					ApiKey!
					);

				string? TextEmbeddingDeploymentName = configuration["AzureAiService:embeddingModel"];
				#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
				kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
					TextEmbeddingDeploymentName!,
					Endpoint!,
					ApiKey!);
				#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

				var searchClient = new SearchClient(
					new Uri(configuration["SearchClient:uri"]!),
					configuration["SearchClient:index"]!,
					new AzureKeyCredential(configuration["SearchClient:key"]!)
					);

				var searchIndexClient = new SearchIndexClient(
					new Uri(configuration["SearchClient:uri"]!), 
					new AzureKeyCredential(configuration["SearchClient:key"]!)
					);

				#pragma warning disable SKEXP0010 
				var TextEmbeddingService = new AzureOpenAITextEmbeddingGenerationService(
					TextEmbeddingDeploymentName!,
					Endpoint!,
					ApiKey!);
				#pragma warning restore SKEXP0010 

				var OpenAiService = new AzureOpenAIChatCompletionService(
					DeploymentName!,
					Endpoint!,
					ApiKey!
					);

				//service;
				kernelBuilder.Services.AddLogging(logging => { logging.AddConsole(); });
				kernelBuilder.Services.AddSingleton<AzureOpenAIChatCompletionService>(OpenAiService);
				kernelBuilder.Services.AddSingleton<SearchClient>(searchClient);
				kernelBuilder.Services.AddSingleton<SearchIndexClient>(searchIndexClient);
				#pragma warning disable SKEXP0010 
				kernelBuilder.Services.AddSingleton<AzureOpenAITextEmbeddingGenerationService>(TextEmbeddingService);
				#pragma warning restore SKEXP0010

				//plugins
				var indexPlugin = new IndexPlugin(searchIndexClient,searchClient,configuration,kernelBuilder.Build());
				kernelBuilder.Plugins.AddFromObject(indexPlugin, "IndexPlugin");

				var customizePlugin = new CustomizePlugin(kernelBuilder.Build(),configuration);
				kernelBuilder.Plugins.AddFromObject(customizePlugin, "CustomizePlugin");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
			return kernelBuilder.Build();
		}

		public AzureOpenAIPromptExecutionSettings RequiredSettings()
		{
			AzureOpenAIPromptExecutionSettings promptExecutionSettings = new AzureOpenAIPromptExecutionSettings
			{
				FunctionChoiceBehavior = FunctionChoiceBehavior.Required()
			};
			return promptExecutionSettings;
		}

	}
}
