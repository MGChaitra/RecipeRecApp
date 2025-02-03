using Azure.Search.Documents.Indexes.Models;

namespace RecipeRec.AzureAiSearch.IndexCreation.Contracts
{
	public interface IAddScoringProfile
	{
		ScoringProfile Add();
	}
}