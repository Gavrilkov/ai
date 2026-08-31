using Microsoft.Extensions.Configuration;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ChatClient : IChatClient
    {
        public ChatClient()
        {
            
        }

        ChatClient GetChatClient()
        {
            var config = GetConfiguration();

            string endpoint = config["AzureOpenAI:Endpoint"];
            string apiKey = config["AzureOpenAI:API_Key"];
            var azureClient = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey)); // Updated class and credential type

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
    }
}
