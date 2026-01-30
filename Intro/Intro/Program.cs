using Azure.AI.OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;
using System.ClientModel;
using OpenAI.Images;


//var a1 = new int[] { 1, 2, 3, 4 };
//var a2 = new int[] { 11, 22 };

//for (int i = 0; i < a1.Count(); i++)
//{
//    for (int j = 0; j < a2.Count(); j++)
//    {
//        if (i == j)
//            Console.WriteLine($"{a1[i]} + {a2[j]} ");
//        else
//            continue;
//    }
//}

var config = GetConfiguration();

string endpoint = config["AzureOpenAI:Endpoint"];
string apiKey = config["AzureOpenAI:API_Key"];

AzureOpenAIClient azureClient = new(new Uri(endpoint), new ApiKeyCredential(apiKey));

UseChatModel(azureClient);
//UseFormattedChatModel(azureClient);
//await UseEmbedingModel(azureClient);
//UseDalle3Model(azureClient);




IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .Build();
}


void UseChatModel(AzureOpenAIClient azureClient)
{
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

    //var response = chatClient.CompleteChatStreaming(messages);
    //PrintResponseToConsole(response);

    var response = chatClient.CompleteChat(messages, requestOptions);
    Console.WriteLine(response.Value.Content[0].Text);

    //// Append the model response to the chat history.
    //messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));
    //// Append new user question.
    //messages.Add(new UserChatMessage("What is so great about #1?"));

    //response = chatClient.CompleteChat(messages);
    //Console.WriteLine(response.Value.Content[0].Text);
}

void UseFormattedChatModel(AzureOpenAIClient azureClient)
{
    ChatClient chatClient = azureClient.GetChatClient(config["AzureOpenAI:Model"]);

    List<ChatMessage> messages = new List<ChatMessage>()
    {
        new UserChatMessage("How can I solve 8x + 7 = -23?"),
    };

    ChatCompletionOptions options = new()
    {
        ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName: "math_reasoning",
            jsonSchema: BinaryData.FromBytes(
                System.Text.Encoding.UTF8.GetBytes(
                """
                {
                    "type": "object",
                    "properties": {
                        "steps": {
                            "type": "array",
                            "items": {
                                "type": "object",
                                "properties": {
                                    "explanation": { "type": "string" },
                                    "output": { "type": "string" }
                                },
                                "required": ["explanation", "output"],
                                "additionalProperties": false
                            }
                        },
                        "final_answer": { "type": "string" }
                    },
                    "required": ["steps", "final_answer"],
                    "additionalProperties": false
                }
                """
                )
            ),
            jsonSchemaIsStrict: true
        )
    };


    ChatCompletion response = chatClient.CompleteChat(messages, options);
    Console.WriteLine(response.Content[0].Text);
}

async Task UseEmbedingModel(AzureOpenAIClient azureClient)
{
    var embeddingClient = azureClient.GetEmbeddingClient("text-embedding-ada-002");
    var embeddingResponse = embeddingClient.GenerateEmbedding("What is the capital of Sweden?");

    ReadOnlyMemory<float> embedding = embeddingResponse.Value.ToFloats();

    Console.Write(string.Join(',', embedding.ToArray()));

}

void UseDalle3Model(AzureOpenAIClient azureClient)
{
    string endpoint2 = config["AzureOpenAI:Endpoint2"];
    string apiKey2 = config["AzureOpenAI:API_Key2"];
    AzureOpenAIClient azureClient2 = new(new Uri(endpoint2), new ApiKeyCredential(apiKey2));


    ImageClient imageClient = azureClient2.GetImageClient("dall-e-3");
    var prompt = "A futuristic city skyline at sunset, with flying cars and neon lights. In the center of frame there is a stone anciant pyramide. ";

    var response = imageClient.GenerateImage(
        prompt,
        new ImageGenerationOptions()
        {
            Size = GeneratedImageSize.W1024xH1024,
            Style = GeneratedImageStyle.Vivid,
            Quality = GeneratedImageQuality.Standard,
            ResponseFormat = GeneratedImageFormat.Uri
        }
    );

    Console.WriteLine(response.Value.ImageUri);
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