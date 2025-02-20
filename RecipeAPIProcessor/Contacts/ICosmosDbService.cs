using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RecipeAPIProcessor.Contacts
{
    public interface ICosmosDbService
    {
        Task AddRecipeAsync(FavoriteRecipeModel recipe);
        Task DeleteRecipeAsync(string recipeName);
        Task<FavoriteRecipeModel> GetRecipeByNameAsync(string recipeName);
        Task<List<FavoriteRecipeModel>> GetRecipesAsync();
    }
}
