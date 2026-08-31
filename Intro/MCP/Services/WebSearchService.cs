namespace MCP.Services
{
    public class WebSearchService
    {
        public WebSearchService()
        {

        }

        public string GetWeatherForecastByCity(string city)
        {
            switch (city)
            {
                case "Seattle":
                    return "The weather is rainy with a high of 60°F.";
                case "New York":
                    return "The weather is cloudy with a high of 70°F.";
                case "Los Angeles":
                    return "The weather is sunny with a high of 85°F.";
                case "Chicago":
                    return "The weather is windy with a high of 65°F.";
                case "Minsk":
                    return "The weather is humid with a high of 90°F. Snow is expected tommorow.";
                default:
                    return "The weather data for the specified city is not available.";

            }
        }
    }
}
