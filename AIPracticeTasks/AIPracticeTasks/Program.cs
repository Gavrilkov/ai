using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.ClientModel;

var config = GetConfiguration();

var endpoint = config["AzureOpenAI:Endpoint"];
var apiKey = config["AzureOpenAI:API_Key"];
var modelName = config["AzureOpenAI:Model"];
modelName = "gemini-2.5-pro";

AzureOpenAIClient aiClient = new(new Uri(endpoint), new ApiKeyCredential(apiKey));
UseChatModel(aiClient);



void UseChatModel(AzureOpenAIClient azureClient)
{
    ChatClient chatClient = azureClient.GetChatClient(modelName);

    var requestOptions = new ChatCompletionOptions()
    {
        //MaxOutputTokenCount = 1024,
        Temperature = 1.0f,
        TopP = 1.0f,
        Seed = 10000,
        FrequencyPenalty =0.0f,
        PresencePenalty = -2.0f,
     //   StopSequences = { "black", "color" },
    };

    List<ChatMessage> messages = new List<ChatMessage>()
{
    new SystemChatMessage("You are a helpful assistant."),
    new UserChatMessage("What is an entropy in LLM's responses?"),
};

    var response = chatClient.CompleteChat(messages, requestOptions);
    Console.WriteLine($"Finish reason: '{response.Value.FinishReason}'");
    Console.WriteLine(response.Value.Content[0].Text);

    // Append the model response to the chat history.
//    messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));
    // Append new user question.
//    messages.Add(new UserChatMessage("What else they can do?"));

//    response = chatClient.CompleteChat(messages);
//    Console.WriteLine(response.Value.Content[0].Text);
}


IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .Build();
}