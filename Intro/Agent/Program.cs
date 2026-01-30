using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using System.ClientModel;
using Agent;
using Agent.ChatTools;
using Agent.Services;
using OpenAI.Chat;

var chatClient = GetChatClient();
var chatAssistant = new ChatAssistant(chatClient,
    new UserServiceToolsProvider(new UserService()),
    new WebSearchServiceToolsProvider(new WebSearchService()));

while (true)
{
    Console.WriteLine("Enter a user management command (or 'exit' to quit):");
    var userMessage = Console.ReadLine();
    if (userMessage?.ToLower() == "exit")
        break;


    Console.WriteLine($"\nUser: {userMessage}");
    var response = await chatAssistant.ChatWithToolsAsync(userMessage);
    Console.WriteLine($"Assistant: {response}");
    Console.WriteLine(new string('-', 50));
}

ChatClient GetChatClient()
{
    var config = GetConfiguration();

    string endpoint = config["AzureOpenAI:Endpoint"];
    string apiKey = config["AzureOpenAI:API_Key"];
    var azureClient = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));

    return azureClient.GetChatClient(config["AzureOpenAI:Model"]);
}

IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .Build();
}