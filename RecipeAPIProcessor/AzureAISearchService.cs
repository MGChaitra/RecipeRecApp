using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Models;

public class AzureAISearchService
{
    private readonly SearchIndexClient _indexClient;
    private readonly SearchClient _searchClient;
    private readonly string _indexName = "recipe";
    private readonly string _endpoint;
    public AzureAISearchService(string searchServiceName, string apiKey, string endpoint)
    {
        _endpoint = endpoint;
        _indexClient = new SearchIndexClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        _searchClient = new SearchClient(new Uri(endpoint), _indexName, new AzureKeyCredential(apiKey));
    }

    /// <summary>
    /// Creates the Azure AI Search index if it does not exist.
    /// </summary>
    public async Task CreateIndexAsync()
    {
        var definition = new SearchIndex(_indexName)
        {
            Fields = new[]
            {
                new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                new SearchableField("recipe_name") { IsFacetable = true },
                new SearchableField("ingredients") { IsFilterable=true },
                new SearchableField("instructions"),
                new SearchField("embedding", SearchFieldDataType.Collection(SearchFieldDataType.Double)),


            }
        };

        await _indexClient.CreateOrUpdateIndexAsync(definition);
    }

    /// <summary>
    /// Searches for recipes in Azure AI Search.
    /// </summary>
    public async Task<List<RecipeModel>> SearchRecipesAsync(string query)
    {
        try
        {
            var searchOptions = new SearchOptions
            {
                IncludeTotalCount = true,
                QueryType = SearchQueryType.Full,
                SearchMode = SearchMode.All,
                Size = 4,
                            
            };
            var response = await _searchClient.SearchAsync<RecipeModel>(query,searchOptions);
            var recipes = response.Value.GetResults().Select(x => x.Document).ToList();
            return recipes;
        }
       catch(RequestFailedException ex)
        {
            return new List<RecipeModel>();
        }
    }
}