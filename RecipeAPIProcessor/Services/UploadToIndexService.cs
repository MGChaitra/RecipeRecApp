using Models;
using System.Text.Json;
using System.Text;
using RecipeAPIProcessor.Contacts;

namespace RecipeAPIProcessor.Services
{
    public class UploadToIndexService: IUploadToIndexService
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly string _apiKey;
    
        public UploadToIndexService(HttpClient httpClient, string endpoint, string apiKey)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
            _apiKey = apiKey;
        }
        public async Task<bool> UploadRecipesToAzureSearch(List<RecipeModel> recipes)
        {
            var payload = new { value = recipes };

            string jsonContent = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("api-key", _apiKey);

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Recipes successfully indexed.");
                return true;
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to index recipes: {errorMessage}");
                return false;
            }
        }
    }
}
