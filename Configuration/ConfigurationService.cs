using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public class ConfigurationService
    {

        // Load configuration
        IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<ConfigurationService>()
            .Build();

        public string GetAzureSearchServiceName() => _configuration["Azure:Search:ServiceName"]!;
        public string GetAzureSearchApiKey() => _configuration["Azure:Search:ApiKey"]!;
        public string GetAzureSearchEndpoint() => _configuration["Azure:Search:Endpoint"]!;
        public string GetAzureSearchUploadEndpoint() => _configuration["Azure:Search:UploadEndpoint"]!;

        public string GetCosmosDbEndpoint() => _configuration["CosmosDb:Endpoint"]!;
        public string GetCosmosDbKey() => _configuration["CosmosDb:Key"]!;
        public string GetCosmosDbDatabaseName() => _configuration["CosmosDb:DatabaseName"]!;
        public string GetCosmosDbContainerName() => _configuration["CosmosDb:ContainerName"]!;

        public string GetAzureOpenAIDeploymentName() => _configuration["AzureOpenAI:DeploymentName"]!;
        public string GetAzureOpenAIEndpoint() => _configuration["AzureOpenAI:Endpoint"]!;
        public string GetAzureOpenAIApiKey() => _configuration["AzureOpenAI:ApiKey"]!;
        public string GetPromptFilePath(string key) => _configuration[$"Prompts:{key}"]!;

    }

}