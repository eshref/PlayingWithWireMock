using System;
using System.Text;
using System.Threading.Tasks;

using WireMock;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Settings;
using WireMock.Types;
using WireMock.Util;


namespace ConsoleApp1
{
    public class CustomResponseProvider : IResponseProvider
    {
        private int _count;

        public Task<(IResponseMessage Message, IMapping Mapping)> ProvideResponseAsync(IMapping mapping, IRequestMessage requestMessage, WireMockServerSettings settings)
        {
            IResponseMessage response = this.createResponse(_count % 2 == 0 ? 200 : 500);

            _count++;

            (IResponseMessage, IMapping) tuple = (response, null);
            
            return Task.FromResult(tuple);
        }


        private IResponseMessage createResponse(int statusCode)
        {
            return new ResponseMessage
            {
                BodyDestination = BodyDestinationFormat.Json,
                StatusCode = statusCode,
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
    }
}