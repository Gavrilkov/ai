using OpenAI.Chat;
namespace Agent.ChatTools
{
    public interface IChatToolProvider
    {
        List<ChatTool> GetChatTools();

        string CallChatTool(ChatToolCall toolCall);
    }
}