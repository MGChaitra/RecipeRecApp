﻿using System.Text.Json.Serialization;

namespace Models
{
	public class IngredientModel
	{
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("food_group")]
		public string food_group { get; set; } = string.Empty;
		public bool Visible { get; set; } = true;
		public bool Selected { get; set; } = false;
	}
}
