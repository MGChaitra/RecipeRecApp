
using Azure.Search.Documents.Indexes;

namespace Models
{
	public class RecipeIndexModel
	{
		[SearchableField(AnalyzerName = "standard.lucene", IsKey = true)]
		public string Id { get; set; }

		[SearchableField(AnalyzerName = "standard.lucene")]
		public string Name { get; set; }

		[SearchableField(AnalyzerName = "standard.lucene")]
		public string Description { get; set; }

		[SearchableField(AnalyzerName = "standard.lucene", IsFilterable = true)]
		public List<string> Ingredients { get; set; }

		[SearchableField(AnalyzerName = "standard.lucene")]
		public List<string> Instructions { get; set; }

		[SimpleField(IsFilterable = true)]
		public bool IsVeg { get; set; }

		[VectorSearchField(VectorSearchDimensions = 1536, VectorSearchProfileName = "my-vector-profile", IsHidden = true)]
		public List<float> RecipeEmbeddings { get; set; }

	}
}
