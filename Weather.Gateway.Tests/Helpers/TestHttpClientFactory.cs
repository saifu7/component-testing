using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Http;

namespace Weather.Gateway.Tests.Helpers
{
    // simple HttpClientFactory used in tests so we can inject HttpClient instances with proxy
    public class TestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public TestHttpClientFactory(string proxyUrl)
        {
            // Create handler using the WireMock proxy
            var handler = new HttpClientHandler
            {
                // WireMock will run on http, so set proxy to http://localhost:PORT
                Proxy = new WebProxy(proxyUrl),
                UseProxy = true,
            };

            // IMPORTANT: Some servers require TLS/SSL usage — HttpClientHandler default behaviour is fine.
            _client = new HttpClient(handler, disposeHandler: true)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public HttpClient CreateClient(string name)
        {
            // return a client that uses WireMock as a proxy for all outbound requests
            return _client;
        }
    }
}
