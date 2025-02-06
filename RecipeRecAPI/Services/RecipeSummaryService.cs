using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Models;

namespace RecipeRecAPI.Services
{
    public class RecipeSummaryService
    {
        private readonly Kernel _kernel;
        private readonly ChatHistory _history;
        private readonly IChatCompletionService _chatCompletionService;

        public RecipeSummaryService(Kernel kernel, ChatHistory history, IChatCompletionService chatCompletionService)
        {
            _kernel = kernel;
            _history = history;
            _history.AddSystemMessage("You are an expert chef. Enhance the following recipe by providing a detailed, step-by-step cooking guide with additional cooking tips.\r\n");
            _chatCompletionService = chatCompletionService;
        }
        public async Task<string> GenerateSummaryAsync(string instructions)
        {

            _history.AddUserMessage(instructions);
           

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                _history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: _kernel);

            return result.Content!;

        }

    }
}
