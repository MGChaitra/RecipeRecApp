using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Configuration;
using RecipeRec.AzureAiSearch.IndexCreation.Contracts;

namespace RecipeRec.AzureAiSearch.IndexCreation.Services
{
	public class AddScoringProfile(IConfiguration configuration) : IAddScoringProfile
	{
		private readonly IConfiguration configuration = configuration;
		public ScoringProfile Add()
		{
			IDictionary<string, double> Weights = new Dictionary<string, double>();
			Weights.Add(new KeyValuePair<string, double>(configuration["ScoreSettings:FieldValue"]!, Convert.ToDouble(configuration["ScoreSettings:WeightValue"])));

			var scoringProfile = new ScoringProfile("embeddingprofile")
			{
				TextWeights = new TextWeights(Weights)
			};

			return scoringProfile;
		}

	}
}
