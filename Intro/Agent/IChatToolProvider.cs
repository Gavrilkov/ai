using OpenAI.Chat;
namespace Agent
{
    public interface IChatToolProvider
    {
        List<ChatTool> GetChatTools();

        string CallChatTool(ChatToolCall toolCall);
    }
}