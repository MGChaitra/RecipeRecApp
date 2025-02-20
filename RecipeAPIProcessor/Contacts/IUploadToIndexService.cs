using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RecipeAPIProcessor.Contacts
{
    public interface IUploadToIndexService
    {
        Task<bool> UploadRecipesToAzureSearch(List<RecipeModel> recipes);
    }
}
