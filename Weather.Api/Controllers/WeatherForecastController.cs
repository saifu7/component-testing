using Microsoft.AspNetCore.Mvc;
using Weather.Service.Contract;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<WeatherForecast> Get(double latitude, double longitude)
        {
            return await _weatherService.GetWeatherForecastAsync(latitude, longitude);
        }
    }
}
