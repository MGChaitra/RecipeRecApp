using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Represents a recipe, including its name and instructions.
    /// </summary>
    public class RecipeModel
        
    {

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("recipe_name")]
        public string recipe_name { get; set; } = string.Empty;

        [JsonPropertyName("instructions")]
        public string instructions { get; set; } = string.Empty;
    }
}
