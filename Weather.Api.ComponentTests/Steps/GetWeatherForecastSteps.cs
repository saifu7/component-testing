using FluentAssertions;
using TechTalk.SpecFlow;
using Weather.Api.ComponentTests.Drivers;
using Weather.Service.Contract;

namespace Weather.Api.ComponentTests.Steps
{
    [Binding]
    public class GetWeatherForecastSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly WeatherApiFactory _weatherApiFactory;
        private HttpResponseMessage? _response;

        public GetWeatherForecastSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _weatherApiFactory = new WeatherApiFactory();
        }

        [Given(@"the latitude is (.*)")]
        public void GivenTheLatitudeIs(double latitude)
        {
            _scenarioContext["latitude"] = latitude;
        }

        [Given(@"the longitude is (.*)")]
        public void GivenTheLongitudeIs(double longitude)
        {
            _scenarioContext["longitude"] = longitude;
        }

        [When(@"the weather forecast is requested")]
        public async Task WhenTheWeatherForecastIsRequested()
        {
            var latitude = _scenarioContext["latitude"];
            var longitude = _scenarioContext["longitude"];

            var client = _weatherApiFactory.CreateClient();
            _response = await client.GetAsync($"/WeatherForecast?latitude={latitude}&longitude={longitude}");
        }

        [Then(@"the response should be successful")]
        public void ThenTheResponseShouldBeSuccessful()
        {
            _response.Should().NotBeNull();
            _response!.IsSuccessStatusCode.Should().BeTrue();
        }

        [Then(@"the response should contain the weather forecast")]
        public async Task ThenTheResponseShouldContainTheWeatherForecast()
        {
            _response.Should().NotBeNull();
            var forecast = await _response!.Content.ReadAsAsync<WeatherForecast>();
            forecast.Should().NotBeNull();
            forecast.Summary.Should().NotBeNullOrEmpty();
        }
    }
}
