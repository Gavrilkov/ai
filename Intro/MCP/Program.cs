// See https://aka.ms/new-console-template for more information
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.ClientModel;

Console.WriteLine("Hello, World!");



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
