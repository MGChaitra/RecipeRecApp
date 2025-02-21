using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RecipeAPIProcessor.Contacts
{
    /// <summary>
    /// Defines a method for uploading a list of recipes to Azure Search.
    /// </summary>
    public interface IUploadToIndexService
    {
        /// <summary>
        /// Asynchronously uploads a list of recipes to Azure Search.
        /// </summary>
        /// <param name="recipes">The list of recipes to upload.</param>
        /// <returns>A boolean indicating whether the upload was successful.</returns>
        Task<bool> UploadRecipesToAzureSearch(List<RecipeModel> recipes);
    }
}
