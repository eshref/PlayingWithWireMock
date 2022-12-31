using System;
using System.Text;
using System.Threading.Tasks;

using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;
using WireMock.Util;


namespace ConsoleApp1
{
    public class CustomResponse : IResponseProvider
    {
        private int _count;

        public Task<(IResponseMessage Message, IMapping Mapping)> ProvideResponseAsync(IMapping mapping, IRequestMessage requestMessage, WireMockServerSettings settings)
        {
            IResponseMessage response;

            if (_count % 2 == 0)
            {
                response = new ResponseMessage
                {
                    BodyDestination = BodyDestinationFormat.Json,
                    StatusCode = 200,
                    BodyData = new BodyData
                    {
                        Encoding = Encoding.UTF8,
                        BodyAsJson = new
                        {
                            Name = "Ashraf",
                            Surname = "Safarov",
                            BirthDate = DateTime.Now.AddYears(-33)
                        },
                        DetectedBodyType = BodyType.Json
                    }
                };
            }
            else
            {
                response = new ResponseMessage
                {
                    BodyDestination = BodyDestinationFormat.Json,
                    StatusCode = 500,
                    BodyData = new BodyData
                    {
                        Encoding = Encoding.UTF8,
                        BodyAsJson = new
                        {
                            Name = "Ashraf",
                            Surname = "Safarov",
                            BirthDate = DateTime.Now.AddYears(-33)
                        },
                        DetectedBodyType = BodyType.Json
                    }
                };
            }

            _count++;

            (IResponseMessage, IMapping) tuple = (response, null);
            
            return Task.FromResult(tuple);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            WireMockServerSettings settings = new WireMockServerSettings
            {
                //Logger =  new  (),
                Port = 9876
            };

            WireMockServer server = WireMockServer.Start(settings);

            Console.WriteLine("wiremock server started on 9876...");

            var responseProvider = Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "text/plain")
                .WithHeader("key1","value1")
                .WithBody("Hello, world!");

            var customResponseProvider = new CustomResponse();

            var request = Request.Create().WithPath("/hello-world").UsingGet();

            server.Given(request).RespondWith(customResponseProvider);

            Console.ReadLine();

            server.Stop();

            Console.WriteLine("wiremock server stopped, press enter to exit.");

            Console.ReadLine();
        }
    }
}