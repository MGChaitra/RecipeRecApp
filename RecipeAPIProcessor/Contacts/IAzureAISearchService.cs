using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RecipeAPIProcessor.Contacts
{
    /// <summary>
    /// Defines methods for creating an index and searching recipes using Azure AI Search.
    /// </summary>
    public interface IAzureAISearchService
    {
        /// <summary>
        /// Asynchronously creates an index.
        /// </summary>
        Task CreateIndexAsync();

        /// <summary>
        /// Asynchronously searches for recipes based on the provided query.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A list of matching recipes.</returns>
        Task<List<RecipeModel>> SearchRecipesAsync(string query);
    }
}
