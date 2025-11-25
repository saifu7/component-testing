using System;
using System.Threading.Tasks;
using Weather.Gateway.Contract;
using Weather.Service.Contract;

namespace Weather.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherGateway _weatherGateway;

        public WeatherService(IWeatherGateway weatherGateway)
        {
            _weatherGateway = weatherGateway;
        }

        public async Task<WeatherForecast> GetWeatherForecastAsync(double latitude, double longitude)
        {
            var gatewayRequest = new GetWeatherForecastRequest
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var gatewayResponse = await _weatherGateway.GetWeatherForecastAsync(gatewayRequest);

            if (gatewayResponse.Current == null)
            {
                throw new InvalidOperationException("Current weather data is not available.");
            }

            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = gatewayResponse.Current.Temperature,
                Summary = GetSummary(gatewayResponse.Current.Temperature)
            };
        }

        private string GetSummary(double temperatureC)
        {
            if (temperatureC < 0) return "Freezing";
            if (temperatureC < 10) return "Cold";
            if (temperatureC < 20) return "Mild";
            if (temperatureC < 30) return "Warm";
            return "Hot";
        }
    }
}
