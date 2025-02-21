﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Represents a favorite recipe, including its unique identifier, name, and instructions.
    /// </summary>
    public class FavoriteRecipeModel
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("recipe_name")]
        public string recipe_name { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("instructions")]
        public string instructions { get; set; } = string.Empty;
    }
}
