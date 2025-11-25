using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Weather.WebCaller;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace Weather.Api.ComponentTests.Drivers
{
    public class WeatherApiFactory : WebApplicationFactory<Program>
    {
        public WireMockServer WireMockServer { get; }

        public WeatherApiFactory()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

            WireMockServer = WireMockServer.Start(new WireMockServerSettings
            {
                FileSystemHandler = new LocalFileSystemHandler(projectRoot),
                ProxyAndRecordSettings = new ProxyAndRecordSettings
                {
                    Url = "https://api.open-meteo.com",
                    SaveMapping = true,
                    SaveMappingToFile = true,
                    AppendGuidToSavedMappingFile = true,
                    SaveMappingForStatusCodePattern = "*",
                    ExcludedHeaders = new[] { "Host", "traceparent", "Content-Length", "X-Forwarded-For", "X-Forwarded-Host", "X-Forwarded-Port", "X-Forwarded-Proto", "X-Original-For" }
                },
                StartAdminInterface = true,
                ReadStaticMappings = true
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IWebCaller>(new TestWebCaller(WireMockServer.Urls[0]));
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                WireMockServer?.Stop();
                WireMockServer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
