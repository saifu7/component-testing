using System;

namespace Weather.Service.Contract
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public double TemperatureC { get; set; }

        public double TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
