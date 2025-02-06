using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class RecipeModel
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("recipe_name")]
        public string recipe_name { get; set; }
        [JsonPropertyName("ingredients")]
        public string ingredients { get; set; }
        [JsonPropertyName("instructions")]
        public string instructions { get; set; }
    }
}
