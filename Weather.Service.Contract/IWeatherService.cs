using System.Threading.Tasks;

namespace Weather.Service.Contract
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherForecastAsync(double latitude, double longitude);
    }
}
