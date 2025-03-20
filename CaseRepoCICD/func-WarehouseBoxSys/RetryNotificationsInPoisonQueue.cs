using System;
using System.Net.Http;
using System.Text;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Polly.Retry;
using Polly;
using Azure;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using func_WarehouseBoxSys.Models;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using func_WarehouseBoxSys.Services;

namespace func_WarehouseBoxSys
{
    public class RetryNotificationsInPoisonQueue
    {
        private readonly ILogger<RetryNotificationsInPoisonQueue> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly IAzureQueueHelper _azureQueueHelper;
        private readonly IMailService _mailService;
        private string processQueueName;
        private string failedQueueName;

        public RetryNotificationsInPoisonQueue(ILogger<RetryNotificationsInPoisonQueue> logger,
                HttpClient httpClient,
                IConfiguration configuration,
                IAzureQueueHelper azureQueueHelper,
                IMailService mailService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _azureQueueHelper = azureQueueHelper;
            _mailService = mailService;

            // Define a retry policy with Polly
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                    });

        }

        [Function(nameof(RetryNotificationsInPoisonQueue))]
        public async Task Run([QueueTrigger("%PoisonQueueName%", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            try
            {
                _logger.LogInformation($"RetryNotificationsInPoisonQueue Queue trigger function processed: {message.MessageText}");

                // Get the API endpoint from configuration
                var apiUrl = _configuration["TrackingStatusUpdateAPIUrl"];

                //make api call to update status with the tracking number
                var content = new StringContent(message.MessageText, Encoding.UTF8, "application/json");
                UpdateLegByTrackingNumberRequest updateLegByTrackingNumberRequest = new UpdateLegByTrackingNumberRequest();
                updateLegByTrackingNumberRequest = JsonConvert.DeserializeObject<UpdateLegByTrackingNumberRequest>(message.MessageText);

                // Step 2: Validate the request body
                // Parse the JSON document
                string transaction;
                JsonElement trackingStatus;
                string trackingNumber;
                string carrier;
                string statusDetails;
                JsonElement subStatus;
                string subStatusCode;
                string statusDate;

                using (JsonDocument document = JsonDocument.Parse(message.MessageText))
                {
                    JsonElement root = document.RootElement;

                    // Get the data node
                    if (root.TryGetProperty("data", out JsonElement dataElement))
                    {
                        transaction = dataElement.GetProperty("transaction").GetString();
                        trackingStatus = dataElement.GetProperty("tracking_status");
                        trackingNumber = dataElement.GetProperty("tracking_number").GetString();
                        carrier = dataElement.GetProperty("carrier").GetString();
                        statusDetails = trackingStatus.GetProperty("status_details").GetString();
                        statusDate = trackingStatus.GetProperty("status_date").GetString();
                        subStatus = trackingStatus.GetProperty("substatus");
                        subStatusCode = subStatus.GetProperty("code").GetString();
                        //Popultate the model with the data from the message
                        updateLegByTrackingNumberRequest.TrackingNumber = trackingNumber;
                        updateLegByTrackingNumberRequest.StatusCode = statusDetails;
                        updateLegByTrackingNumberRequest.SubStatusCode = subStatusCode;
                        updateLegByTrackingNumberRequest.TransactionId = transaction;
                        updateLegByTrackingNumberRequest.StatusDate = statusDate;
                        updateLegByTrackingNumberRequest.UpdatedBy = carrier;


                        _logger.LogInformation("Transaction: {Transaction}", transaction);
                        _logger.LogInformation("Tracking Status: {TrackingStatus}", trackingStatus);
                        _logger.LogInformation("Status Date: {statusDate}", trackingStatus);
                        _logger.LogInformation("TrackingNumber: {trackingNumber}", trackingNumber);
                        _logger.LogInformation("Carrier: {carrier}", carrier);
                        _logger.LogInformation("StatusDetails: {statusDetails}", statusDetails);
                        _logger.LogInformation("SubStatusCode: {subStatusCode}", subStatusCode);
                    }
                    else
                    {
                        _logger.LogError("The 'data' node was not found in the JSON document.");
                        return;
                    }
                }

                content = new StringContent(JsonConvert.SerializeObject(updateLegByTrackingNumberRequest), Encoding.UTF8, "application/json");

                // Make the HTTP POST request to on Prem Warehouse BoxSys API
                //var strTrackingDetails = "{\r\n    \"TrackingNumber\": \"283237333336\",\r\n    \"StatusCode\": \"Delivered\",\r\n    \"SubStatusCode\": \"delivered\",\r\n    \"UpdatedBy\": \"JohnDoe\"\r\n}";
                //var content = new StringContent(strTrackingDetails, Encoding.UTF8, "application/json");
                var response = new System.Net.Http.HttpResponseMessage();
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    response = await _httpClient.PostAsync(apiUrl, content);
                });

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully updated tracking status thry WarehoseBoxSys API.");
                    // Send the message to the processed items queue
                    await _azureQueueHelper.AddMessageToProcessedQueue(message.MessageText);
                    _logger.LogInformation("Successfully post the message to the API. Status code: {response.StatusCode}");

                }
                else
                {
                    // Send the message to the Failed Queue items queue
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to post the message to the API. Status code: {response.StatusCode}");
                    _logger.LogError($"Failed API Response Details: {responseContent}");
                    await _azureQueueHelper.AddMessageToFailedQueue(message.MessageText);
                    await _mailService.SendEmailAsync(
                            _configuration["MailTo"],
                            "Failed Notifications",
                            message.MessageText
                    );
                    _logger.LogInformation("Successfully sent the message to the Failed items queue.");
                    _logger.LogError($"Failed to post the message to the API. Status code: {response.StatusCode}");
                    throw new Exception($"Failed to post the message to the API. Status code: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");

                // Optionally, you can send the message to the Failed Queue items queue in case of an exception
                await _azureQueueHelper.AddMessageToFailedQueue(message.MessageText);
                _logger.LogInformation("Successfully sent the message to the Failed items queue due to an exception.");
                throw new Exception($"Failed to execute function UpdateShippoTrackingStatus. Status code: {ex}");
            }
        }
    }
}
