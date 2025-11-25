using System.Net.Http;
using System.Threading.Tasks;
using Weather.Gateway.Contract;
using Weather.WebCaller;

namespace Weather.Gateway
{
    public class WeatherGateway : IWeatherGateway
    {
        private readonly IWebCaller _webCaller;
        private const string OpenMeteoApiUrl = "https://api.open-meteo.com/v1/forecast";

        public WeatherGateway(IWebCaller webCaller)
        {
            _webCaller = webCaller;
        }

        public async Task<GetWeatherForecastResponse> GetWeatherForecastAsync(GetWeatherForecastRequest request)
        {
            var uri = $"{OpenMeteoApiUrl}?latitude={request.Latitude}&longitude={request.Longitude}&current=temperature_2m,windspeed_10m";
            return await _webCaller.SendAsync<GetWeatherForecastResponse>(HttpMethod.Get, uri);
        }
    }
}
