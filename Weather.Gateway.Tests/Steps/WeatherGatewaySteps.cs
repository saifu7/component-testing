using FluentAssertions;
using TechTalk.SpecFlow;      // ← IMPORTANT: SpecFlow namespace
using Weather.Gateway;
using Weather.Gateway.Contract;
using Weather.Gateway.Tests.Helpers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace Weather.Api.Tests.Steps
{
    [Binding]
    public class WeatherGatewaySteps
    {
        private static WireMockServer _server = null!;
        private readonly string _mappingsFolder =
            Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "WireMockMappings");

        private GetWeatherForecastResponse _response = null!;
        private Exception? _error;

        [Given(@"the WireMock proxy is set up to record to ""(.*)""")]
        public void GivenTheWireMockProxyIsSetUpToRecordTo(string folderName)
        {
            var mappingsPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", folderName);
            Directory.CreateDirectory(mappingsPath);

            var url = "http://localhost:9091";

            if (_server == null)
            {
                _server = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { url },
                    StartAdminInterface = true,
                    ReadStaticMappings = true,
                    FileSystemHandler = new WireMock.FileSystemHandler.FileSystemHandler(mappingsPath)
                });
            }
        }

        [When(@"I request a weather forecast for latitude ""(.*)"" and longitude ""(.*)""")]
        public async Task WhenIRequestAWeatherForecastForLatitudeAndLongitude(string lat, string lon)
        {
            try
            {
                var mappingsDir = Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    "..", "..", "..",
                    "WireMockMappings",
                    "mappings"
                );

                bool mappingsExist = Directory.Exists(mappingsDir) &&
                                     Directory.EnumerateFiles(mappingsDir).Any();

                if (!mappingsExist)
                {
                    _server
                        .Given(Request.Create().WithPath("/*").UsingGet())
                        .AtPriority(100)
                        .RespondWith(Response.Create()
                        .WithProxy("https://api.open-meteo.com")
                        .WithTransformer());
                }

                var proxyUrl = _server.Urls.First();
                var httpClientFactory = new TestHttpClientFactory(proxyUrl);

                var webCaller = new WebCaller(httpClientFactory);
                var gateway = new WeatherGateway(webCaller);

                var req = new GetWeatherForecastRequest
                {
                    Latitude = lat,
                    Longitude = lon
                };

                _response = await gateway.GetWeatherForecastAsync(req);

                if (!mappingsExist)
                {
                    try
                    {
                        _server.SaveMappings();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SaveMappings() failed: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _error = ex;
            }
        }

        [Then(@"the gateway returns a valid response")]
        public void ThenTheGatewayReturnsAValidResponse()
        {
            _error.Should().BeNull("because the gateway should not throw exceptions");
            _response.Should().NotBeNull();
            _response.CurrentWeather.Should().NotBeNull();
            _response.CurrentWeather.Temperature_2m.Should().NotBe(0);
        }

        [Then(@"a mapping for that request exists on disk")]
        public void ThenAMappingForThatRequestExistsOnDisk()
        {
            var mappingsDir = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "..", "..", "..",
                "WireMockMappings",
                "mappings"
            );

            for (int i = 0; i < 5 && !Directory.Exists(mappingsDir); i++)
            {
                Task.Delay(200).Wait();
            }

            Directory.Exists(mappingsDir).Should().BeTrue();

            var files = Directory.Exists(mappingsDir)
                ? Directory.EnumerateFiles(mappingsDir).ToArray()
                : Array.Empty<string>();

            files.Length.Should().BeGreaterThan(0);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                _server?.Stop();
                _server?.Dispose();
                _server = null!;
            }
            catch { }
        }
    }
}
