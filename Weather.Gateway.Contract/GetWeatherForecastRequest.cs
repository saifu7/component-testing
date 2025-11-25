namespace Weather.Gateway.Contract
{
    public class GetWeatherForecastRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
