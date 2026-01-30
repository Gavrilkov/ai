using Agent.ChatTools;
using OpenAI.Chat;

namespace Agent
{
    public class ChatAssistant
    {
        private readonly ChatClient _chatClient;
        private readonly IChatToolProvider[] _toolProviders;

        public ChatAssistant(ChatClient chatClient, params IChatToolProvider[] chatToolProviders)
        {
            _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
            _toolProviders = chatToolProviders ?? throw new ArgumentNullException(nameof(chatToolProviders));
        }

        public async Task<string> ChatWithToolsAsync(string userMessage)
        {
            var messages = new List<ChatMessage>
                    {
                        new SystemChatMessage("You are a helpful assistant that can manage users. You have access to user management functions."),
                        new UserChatMessage(userMessage),
                    };

            var chatOptions = new ChatCompletionOptions()
            {
                ToolChoice = ChatToolChoice.CreateAutoChoice()
            };

            // Add tools to the read-only collection
            foreach (var toolProvider in _toolProviders)
            {
                foreach (var tool in toolProvider.GetChatTools())
                {
                    chatOptions.Tools.Add(tool);
                }
            }

            var response = _chatClient.CompleteChat(messages, chatOptions);

            // Handle function calls if any
            while (response.Value.FinishReason == ChatFinishReason.ToolCalls)
            {
                // Add the assistant's message with tool calls to the conversation
                messages.Add(new AssistantChatMessage(response.Value.ToolCalls));

                // Process each tool call
                foreach (var toolCall in response.Value.ToolCalls)
                {
                    if (toolCall.Kind == ChatToolCallKind.Function)
                    {
                        var functionResult = HandleFunctionCall(toolCall);
                        messages.Add(new ToolChatMessage(toolCall.Id, functionResult));
                    }
                }

                // Get the next response after processing tool calls
                response = _chatClient.CompleteChat(messages, chatOptions);
            }

            return response.Value.Content.FirstOrDefault()?.Text ?? "No response generated.";
        }

        private string HandleFunctionCall(ChatToolCall toolCall)
        {

            foreach (var toolProvider in _toolProviders)
            {
                foreach (var tool in toolProvider.GetChatTools())
                {
                    if (tool.FunctionName == toolCall.FunctionName)
                    {
                        return toolProvider.CallChatTool(toolCall);
                    }
                }
            }

            return "Unknown function: " + toolCall.FunctionName;
        }
    }
}
