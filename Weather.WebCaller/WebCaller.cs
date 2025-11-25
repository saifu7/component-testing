using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Weather.WebCaller
{
    public class WebCaller : IWebCaller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WebCaller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> SendAsync<T>(HttpMethod method, string uri, object? content = null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
            {
                request.Content = JsonContent.Create(content);
            }

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<T>();
            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize response content.");
            }
            return result;
        }
    }
}
