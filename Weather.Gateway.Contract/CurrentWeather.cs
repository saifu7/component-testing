using Newtonsoft.Json;

namespace Weather.Gateway.Contract
{
    public class CurrentWeather
    {
        [JsonProperty("temperature_2m")]
        public double Temperature { get; set; }

        [JsonProperty("windspeed_10m")]
        public double Windspeed { get; set; }
    }
}
