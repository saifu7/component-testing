// WireMockServer/WireMockSetup.cs
using WireMock.Admin.Mappings;
using WireMock.Logging;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace Weather.Gateway.Tests.WireMockServer
{
    public class WireMockSetup : IDisposable
    {
        public WireMockServer Server { get; }
        public string Url => Server.Url!;

        public WireMockSetup(bool record = false)
        {
            Server = WireMockServer.Start(new WireMockServerSettings
            {
                StartAdminInterface = true,
                ReadStaticMappings = true,
                AllowPartialMapping = true,
                // This folder will contain saved recordings
                FileSystemHandler = new WireMock.Net.StandAlone.FileSystemHandler("Stubs")
            });

            if (record)
            {
                // RECORD MODE: Proxy all requests to real Open-Meteo
                Server.Given(Request.Create().UsingAnyMethod())
                      .RespondWith(Response.Create()
                          .WithProxyUrl("https://api.open-meteo.com"));

                Console.WriteLine("RECORD MODE ACTIVE - Calling real Open-Meteo API and saving responses...");
            }
            else
            {
                Console.WriteLine("REPLAY MODE - Using saved stubs from ./Stubs");
            }
        }

        public void Dispose()
        {
            Server.Stop();
            Server.Dispose();
        }
    }
}