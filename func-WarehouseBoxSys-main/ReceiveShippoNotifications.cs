using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Functions.Worker.Extensions.Http;
using Microsoft.Azure.Functions.Worker.Pipeline;
using System.Text.Json;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using func_WarehouseBoxSys.Services;
using func_WarehouseBoxSys.Models;

namespace func_WarehouseBoxSys
{
    public class ReceiveShippoNotifications
    {
        private readonly ILogger<ReceiveShippoNotifications> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly IAzureQueueHelper _azureQueueHelper;


        public ReceiveShippoNotifications(ILogger<ReceiveShippoNotifications> logger,
                                            IConfiguration configuration,
                                            IAzureQueueHelper azureQueueHelper)
        {
            _logger = logger;
            var connectionString = configuration["AzureWebJobsStorage"];
            var strNotificationQueueName = configuration["NotificationQueueName"];

            _azureQueueHelper = azureQueueHelper;

            // Define a retry policy with Polly
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                    });
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        [Function(nameof(ReceiveShippoNotifications))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext) // Removed the queueMessage parameter
        {

            try
            {
                _logger.LogInformation("ReceiveShippoNotifications HTTP trigger function processed a request.");
                string requestBody = null!;

                // Retry reading the request body
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                });

                // Create a message to be added to the queue
                //TimeSpan delay = TimeSpan.FromMinutes(1); // Delay of 1 minute

                await _azureQueueHelper.AddMessageToNotificationsQueue(requestBody);

                // Create the response
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync($"Received data: {requestBody}");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");

                // Create the error response
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await errorResponse.WriteStringAsync("An error occurred while processing your request.");

                return errorResponse;
            }

        }
    }

}

