using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;


namespace AWC.Function
{
    public class WebSocket
    {
        private readonly ILogger _logger;

        public WebSocket(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiate");
        }

        [Function("negotiate")]
        public async Task<HttpResponseData> Negotiate(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "serverless", UserId = "{headers.userid}")] MyConnectionInfo connectionInfo)
        {
            var userId = req.Headers.FirstOrDefault(h => h.Key == "userid").Value.FirstOrDefault();
            _logger.LogInformation($"Connected to {userId}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { url = connectionInfo.Url, accessToken = connectionInfo.AccessToken });

            return response;
        }

        /* 
        A test function to test sending a message to a particular user using SignalR before implementing in blob trigger

                [Function("broadcast")]
                [SignalROutput(HubName = "serverless")]
                public SignalRMessageAction Broadcast(
                    [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req
                ) {
                    var userId = req.Headers.FirstOrDefault(h => h.Key == "userid").Value.FirstOrDefault();
                    _logger.LogInformation($"Broadcasting to {userId}");
                    SignalRMessageAction action = new SignalRMessageAction("newMessage", ["Hello from serverless function"])
                    {
                        UserId = userId
                    };

                    return action;

                }
                */
    }

    public class MyConnectionInfo
    {
        public string Url { get; set; }

        public string AccessToken { get; set; }
    }
}