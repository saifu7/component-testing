using System.Threading.Tasks;

namespace Weather.Gateway.Contract
{
    public interface IWeatherGateway
    {
        Task<GetWeatherForecastResponse> GetWeatherForecastAsync(GetWeatherForecastRequest request);
    }
}
