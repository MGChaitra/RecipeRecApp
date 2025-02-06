using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace RecipeRec.KernelOps.Contracts
{
	public interface IKernalProvider
	{
		Kernel CreateKernal();
		AzureOpenAIPromptExecutionSettings RequiredSettings();
	}
}