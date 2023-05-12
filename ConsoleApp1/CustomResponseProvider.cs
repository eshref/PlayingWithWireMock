using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using WireMock;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Settings;
using WireMock.Types;
using WireMock.Util;


namespace Simbrella.SimKredit.Services.Core.NotificationSender.IntegrationTests.Integration
{
    internal class CustomResponseProvider : IResponseProvider
    {
        private readonly int _failHttpStatusCode;

        private readonly int _failedRequestsCount;

        private readonly TimeSpan _delayPeriod;

        private readonly int _timeoutedRequestsCount;


        private int _currentRequestCount;

        private int _currentTimeoutedRequestsCount;


        public CustomResponseProvider(
            int failHttpStatusCode,
            int failedRequestsCount,
            TimeSpan delayPeriod,
            int timeoutedRequestsCount)
        {
            _failHttpStatusCode = failHttpStatusCode;

            _failedRequestsCount = failedRequestsCount;

            _delayPeriod = delayPeriod;

            _timeoutedRequestsCount = timeoutedRequestsCount;
        }


        public async Task<(IResponseMessage Message, IMapping Mapping)> ProvideResponseAsync(
            IMapping mapping,
            IRequestMessage requestMessage,
            WireMockServerSettings settings)
        {
            if (_delayPeriod > TimeSpan.Zero && _timeoutedRequestsCount > _currentTimeoutedRequestsCount)
            {
                await Task.Delay(_delayPeriod);

                (IResponseMessage, IMapping) timeoutResponse = (new ResponseMessage
                {
                    StatusCode = HttpStatusCode.RequestTimeout
                }, mapping);

                _currentTimeoutedRequestsCount++;

                return await Task.FromResult(timeoutResponse);
            }

            IResponseMessage response = _failedRequestsCount > _currentRequestCount
                ? this.createResponse(_failHttpStatusCode, "Failed")
                : this.createResponse(200, "OK");

            _currentRequestCount++;

            (IResponseMessage, IMapping) result = (response, mapping);

            return await Task.FromResult(result);
        }


        private ResponseMessage createResponse(int statusCode, string resultDescription)
        {
            return new ResponseMessage
            {
                BodyDestination = BodyDestinationFormat.Json,
                StatusCode = statusCode,
                BodyData = new BodyData
                {
                    Encoding = Encoding.UTF8,
                    BodyAsJson = new ApiResponse { ResultCode = statusCode, ResultDescription = resultDescription },
                    DetectedBodyType = BodyType.Json
                }
            };
        }
    }
}
