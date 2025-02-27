﻿using Models;
using RecipeRecWebApp.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace RecipeRecWebApp.Services
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="httpclient"></param>
	/// <param name="logger"></param>
	public class IngredientService(HttpClient httpclient, ILogger<IngredientService> logger) : IIngredientService
	{
		private readonly HttpClient _httpClient = httpclient;
		private readonly ILogger<IngredientService> logger = logger;

		/// <summary>
		/// Async Method to get ingredients from the API
		/// </summary>
		/// <returns>List of Ingredients</returns>
		public async Task<List<IngredientModel>> GetIngredientsAsync()
		{
			List<IngredientModel> ingredients = [];
			try
			{
				ingredients = await _httpClient.GetFromJsonAsync<List<IngredientModel>>("api/Ingredient") ?? [];
				foreach (var ingredient in ingredients)
				{
					ingredient.Selected = false;
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
			return ingredients;
		}

		/// <summary>
		///	Async Method to add a new ingredient via the API
		/// </summary>
		/// <param name="ingredient"></param>
		/// <returns>bool - did ingredient got added</returns>
		public async Task<bool> AddIngredientAsync(IngredientModel ingredient)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/Ingredient", ingredient);
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
				return false;
			}
		}


		

		public void MapIngredients()
		{
			try
			{
				SharedDataModel.Categories = SharedDataModel.Ingredients
									.GroupBy(ingredient => ingredient.food_group, StringComparer.OrdinalIgnoreCase)
									.Select(group => new CategoryModel
									{
										Name = group.Key,
										Ingredients = [.. group],
										IsExpanded = false
									})
									.ToList();
			}
			catch (Exception ex)
			{
				logger.LogError($"Error: {ex.Message}");
			}
		}

	}
}