using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RecipeAPIProcessor.Contacts
{
    public interface IAzureAISearchService
    {
        Task CreateIndexAsync();
        Task<List<RecipeModel>> SearchRecipesAsync(string query);
    }
}
