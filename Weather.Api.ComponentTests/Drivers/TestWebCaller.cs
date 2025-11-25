using Weather.WebCaller;

namespace Weather.Api.ComponentTests.Drivers
{
    public class TestWebCaller : IWebCaller
    {
        private readonly string _wireMockUrl;
        private readonly HttpClient _httpClient;

        public TestWebCaller(string wireMockUrl)
        {
            _wireMockUrl = wireMockUrl;
            _httpClient = new HttpClient();
        }

        public async Task<T> SendAsync<T>(HttpMethod method, string uri, object? content = null)
        {
            var originalUri = new Uri(uri);
            //var proxyUri = new UriBuilder(uri)
            //{
            //    Scheme = new Uri(_wireMockUrl).Scheme,
            //    Host = new Uri(_wireMockUrl).Host,
            //    Port = new Uri(_wireMockUrl).Port,
            //    Path = originalUri.AbsolutePath,
            //    Query = originalUri.Query.Substring(1)
            //}.Uri.ToString();
            var proxyUri = new UriBuilder(_wireMockUrl)
            {
                Path = originalUri.AbsolutePath,
                Query = originalUri.Query.TrimStart('?')
            }.Uri;
            var request = new HttpRequestMessage(method, proxyUri);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<T>();
        }
    }
}
