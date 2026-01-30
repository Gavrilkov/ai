using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using System.ClientModel;
using Agent;

var config = GetConfiguration();

string endpoint = config["AzureOpenAI:Endpoint"];
string apiKey = config["AzureOpenAI:API_Key"];

AzureOpenAIClient azureClient = new(new Uri(endpoint), new ApiKeyCredential(apiKey));

var chatClient = azureClient.GetChatClient(config["AzureOpenAI:Model"]);
var userServiceChatTool = new ChatAssistant(chatClient, new UserServiceToolsProvider(new UserService()));

while (true)
{
    Console.WriteLine("Enter a user management command (or 'exit' to quit):");
    var userMessage = Console.ReadLine();
    if (userMessage?.ToLower() == "exit")
        break;


    Console.WriteLine($"\nUser: {userMessage}");
    var response = await userServiceChatTool.ChatWithToolsAsync(userMessage);
    Console.WriteLine($"Assistant: {response}");
    Console.WriteLine(new string('-', 50));
}

IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .Build();
}