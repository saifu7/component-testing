Feature: WeatherGateway recording and replay
  As a component test
  I want to record the 3rd-party response once and then replay it on subsequent runs

  Scenario: Get weather forecast records first time and replays afterwards
    Given the WireMock proxy is set up to record to "WireMockMappings"
    When I request a weather forecast for latitude "52.52" and longitude "13.41"
    Then the gateway returns a valid response
    And a mapping for that request exists on disk
