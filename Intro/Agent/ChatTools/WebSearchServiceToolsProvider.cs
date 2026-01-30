using OpenAI.Chat;
using System.Text.Json;
using Agent.Services;

namespace Agent.ChatTools
{
    public class WebSearchServiceToolsProvider : IChatToolProvider
    {
        private readonly WebSearchService _webSearchService;

        public WebSearchServiceToolsProvider(WebSearchService webSearchService)
        {
            _webSearchService = webSearchService ?? throw new ArgumentNullException(nameof(webSearchService));
        }

        public List<ChatTool> GetChatTools()
        {
            return new List<ChatTool>
            {
                ChatTool.CreateFunctionTool(
                    functionName: "get_weather_forecast_by_city",
                    functionDescription: "Get weather forecast for a specific city",
                    functionParameters: BinaryData.FromString("""
                    {
                        "type": "object",
                        "properties": {
                            "city": {
                                "type": "string",
                                "description": "The name of the city to get weather forecast for"
                            }
                        },
                        "required": ["city"]
                    }
                    """)
                )
            };
        }

        public string CallChatTool(ChatToolCall toolCall)
        {
            var functionName = toolCall.FunctionName;
            var arguments = toolCall.FunctionArguments.ToString();

            return functionName switch
            {
                "get_weather_forecast_by_city" => HandleGetWeatherForecastByCity(arguments),
                _ => $"Unknown function: {functionName}"
            };
        }

        private string HandleGetWeatherForecastByCity(string arguments)
        {
            try
            {
                var args = JsonSerializer.Deserialize<JsonElement>(arguments);
                var city = args.GetProperty("city").GetString() ?? string.Empty;

                var weatherForecast = _webSearchService.GetWeatherForecastByCity(city);
                return weatherForecast;
            }
            catch (Exception ex)
            {
                return $"Error getting weather forecast: {ex.Message}";
            }
        }
    }
}
