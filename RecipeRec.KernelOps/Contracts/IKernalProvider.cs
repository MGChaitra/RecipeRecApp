using Microsoft.SemanticKernel;

namespace RecipeRec.KernelOps.Contracts
{
	public interface IKernalProvider
	{
		Kernel CreateKernal();
		PromptExecutionSettings RequiredSettings();
	}
}