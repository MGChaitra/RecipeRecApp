using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Models;
using Microsoft.Extensions.Logging;
using RecipeAPIProcessor.Contacts;

public class AzureAISearchService: IAzureAISearchService
{
    private readonly SearchIndexClient _indexClient;
    private readonly SearchClient _searchClient;
    private readonly string _indexName = "recipenew";
    private readonly string _endpoint;
    private readonly ILogger<AzureAISearchService> _logger;
    public AzureAISearchService(string searchServiceName, string apiKey, string endpoint, ILogger<AzureAISearchService> logger)
    {
        _endpoint = endpoint;
        _indexClient = new SearchIndexClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        _searchClient = new SearchClient(new Uri(endpoint), _indexName, new AzureKeyCredential(apiKey));
        _logger = logger;
    }

    /// <summary>
    /// Creates the Azure AI Search index if it does not exist.
    /// </summary>
    public async Task CreateIndexAsync()
    {
        try
        {
            var definition = new SearchIndex(_indexName)
            {
                Fields = new[]
                {
                 
                    new SearchableField("recipe_name") {IsKey = true,  IsFacetable = true},
                    new SearchableField("instructions"),
                    new SearchField("embedding", SearchFieldDataType.Collection(SearchFieldDataType.Double)),
                }
            };

            await _indexClient.CreateOrUpdateIndexAsync(definition);
            _logger.LogInformation("Index created or updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating or updating the index.");
            throw;
        }
    }

    /// <summary>
    /// Searches for recipes in Azure AI Search.
    /// </summary>

    public async Task<List<RecipeModel>> SearchRecipesAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            _logger.LogWarning("Search query is null or empty.");
            return new List<RecipeModel>();
        }

        try
        {
            var searchOptions = new SearchOptions
            {
                IncludeTotalCount = true,
                QueryType = SearchQueryType.Full,
                SearchMode = SearchMode.All,
                Size = 6,
            };

            var response = await _searchClient.SearchAsync<RecipeModel>(query, searchOptions);
            var recipes = response.Value.GetResults().Select(x => x.Document).ToList();
            _logger.LogInformation("Search completed successfully with {Count} results.", recipes.Count);
            return recipes;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Search request failed.");
            return new List<RecipeModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during the search.");
            return new List<RecipeModel>();
        }
    }
}
