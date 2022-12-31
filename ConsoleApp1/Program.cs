using System;

using WireMock.Logging;
using WireMock.RequestBuilders;
using WireMock.Server;
using WireMock.Settings;


namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            WireMockServerSettings settings = new WireMockServerSettings
            {
                Logger = new WireMockConsoleLogger(),
                Port = 9876
            };

            WireMockServer server = WireMockServer.Start(settings);

            Console.WriteLine("wiremock server started on 9876...");

            // var responseProvider = Response.Create()
            //     .WithStatusCode(200)
            //     .WithHeader("Content-Type", "text/plain")
            //     .WithHeader("key1", "value1")
            //     .WithBody("Hello, world!");

            var customResponseProvider = new CustomResponseProvider();

            IRequestBuilder request = Request.Create().WithPath("/hello-world").UsingGet();

            server.Given(request).RespondWith(customResponseProvider);

            Console.ReadLine();

            server.Stop();

            Console.WriteLine("wiremock server stopped, press enter to exit.");

            Console.ReadLine();
        }
    }
}