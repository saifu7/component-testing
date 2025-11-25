using Newtonsoft.Json;

namespace Weather.Gateway.Contract
{
    public class GetWeatherForecastResponse
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("timezone")]
        public string? Timezone { get; set; }

        [JsonProperty("current")]
        public CurrentWeather? Current { get; set; }
    }
}
