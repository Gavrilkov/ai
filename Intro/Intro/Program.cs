using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;
using System.ClientModel;


var config = GetConfiguration();

string endpoint = config["AzureOpenAI:Endpoint"];
string apiKey = config["AzureOpenAI:API_Key"];

AzureOpenAIClient azureClient = new(new Uri(endpoint), new ApiKeyCredential(apiKey));
ChatClient chatClient = azureClient.GetChatClient(config["AzureOpenAI:Model"]);

var requestOptions = new ChatCompletionOptions()
{
    MaxOutputTokenCount = 1024,
    Temperature = 1.0f,
    TopP = 1.0f,

};

List<ChatMessage> messages = new List<ChatMessage>()
{
    new SystemChatMessage("You are a helpful assistant."),
    new UserChatMessage("I am going to Stokholm, what should I see?"),
};

var response = chatClient.CompleteChatStreaming(messages);
PrintResponseToConsole(response);

//var response = await chatClient.CompleteChatAsync(messages, requestOptions);
//Console.WriteLine(response.Value.Content[0].Text);

//// Append the model response to the chat history.
//messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));
//// Append new user question.
//messages.Add(new UserChatMessage("What is so great about #1?"));

//response = chatClient.CompleteChat(messages);
//Console.WriteLine(response.Value.Content[0].Text);







IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //.AddUserSecrets<Startup>()
        .Build();
}

void PrintResponseToConsole(CollectionResult<StreamingChatCompletionUpdate> response)
{

    foreach (StreamingChatCompletionUpdate update in response)
    {
        foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
        {
            Console.Write(updatePart.Text);
        }
    }
    Console.WriteLine("");
}