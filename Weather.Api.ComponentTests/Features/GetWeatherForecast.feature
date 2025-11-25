Feature: GetWeatherForecast
	As a user
	I want to get the weather forecast for a given location
	So that I can know the weather

Scenario: Successfully retrieve weather forecast
	Given the latitude is 52.52
	And the longitude is 13.41
	When the weather forecast is requested
	Then the response should be successful
	And the response should contain the weather forecast

Scenario: Successfully retrieve weather forecast for a different location
	Given the latitude is 40.71
	And the longitude is -74.01
	When the weather forecast is requested
	Then the response should be successful
	And the response should contain the weather forecast
