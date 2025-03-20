
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace func_WarehouseBoxSys.Services;
public class AzureQueueHelper : IAzureQueueHelper
{
    private readonly QueueServiceClient _queueServiceClient;
    private readonly IConfiguration _configuration;

    public AzureQueueHelper(QueueServiceClient queueServiceClient, 
                            IConfiguration configuration)
    {
        _queueServiceClient = queueServiceClient;
        _configuration = configuration;
    }
    public async Task AddMessageToNotificationsQueue(string data)
    {
        var notificationsQueueName = _configuration["NotificationQueueName"];
        if (string.IsNullOrWhiteSpace(notificationsQueueName))
        {
            throw new ArgumentException("NotificationQueueName configuration is missing or empty.");
        }
        await SendMessagetoQueue(data, notificationsQueueName);
    }
    public async Task AddMessageToProcessedQueue(string data)
    {
        var processQueueName = _configuration["ProcessedQueueName"];
        if (string.IsNullOrWhiteSpace(processQueueName))
        {
            throw new ArgumentException("ProcessedQueueName configuration is missing or empty.");
        }
        await SendMessagetoQueue(data, processQueueName);
    }
    public async Task AddMessageToFailedQueue(string data)
    {
        var failedQueueName = _configuration["FailedQueueName"];
        if (string.IsNullOrWhiteSpace(failedQueueName))
        {
            throw new ArgumentException("FailedQueueName configuration is missing or empty.");
        }
        await SendMessagetoQueue(data, failedQueueName);
    }
    public async Task AddMessageToManualQueue(string data)
    {
        var manualQueueName = _configuration["MnaualQueueName"];
        if (string.IsNullOrWhiteSpace(manualQueueName))
        {
            throw new ArgumentException("ManualQueueName configuration is missing or empty.");
        }
        await SendMessagetoQueue(data, manualQueueName);
    }

    private async Task SendMessagetoQueue(string data, string queueName)
    {
        try
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.SendMessageAsync(Base64Encode(data), default, TimeSpan.FromSeconds(-1), default);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to send message to queue '{queueName}'.", ex);
        }
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

}

