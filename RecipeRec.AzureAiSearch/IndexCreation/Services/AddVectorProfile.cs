using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Configuration;
using RecipeRec.AzureAiSearch.IndexCreation.Contracts;

namespace RecipeRec.AzureAiSearch.IndexCreation.Services
{
	public class AddVectorProfile(IConfiguration configuration) : IAddVectorProfile
	{
		private readonly IConfiguration configuration = configuration;

		public VectorSearch Add()
		{
			var vectorProfile = new VectorSearch()
			{
				Algorithms =
					{
						new HnswAlgorithmConfiguration("hnsw")
						{
							Parameters = new HnswParameters()
							{
								M = Convert.ToInt32(configuration["IndexSettings:biDirectionalLink(M)"]),
								EfConstruction = Convert.ToInt32(configuration["IndexSettings:EfConstruct"]),
								EfSearch = Convert.ToInt32(configuration["IndexSettings:EfSearch"])
							}
						}
					},
				Profiles =
					{
						new VectorSearchProfile("my-vector-profile", "hnsw")
						{
							VectorizerName = "my-vectorizer"
						}
					},
				Vectorizers =
					{
						new AzureOpenAIVectorizer("my-vectorizer")
						{
							Parameters = new AzureOpenAIVectorizerParameters()
							{
								ApiKey = configuration["AzureAiService:key"],
								DeploymentName = configuration["AzureAiService:embeddingModel"],
								ModelName = configuration["AzureAiService:embeddingModel"],
								ResourceUri = new Uri(configuration["AzureAiService:endpoint"]!)
							}
						}
					}

			};
			return vectorProfile;
		}
	}
}
