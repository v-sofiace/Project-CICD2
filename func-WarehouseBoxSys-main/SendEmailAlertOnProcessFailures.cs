using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using func_WarehouseBoxSys.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.SafeHandles;
using System.Configuration;

namespace func_WarehouseBoxSys
{
    public class SendEmailAlertOnProcessFailures
    {
        private readonly ILogger<SendEmailAlertOnProcessFailures> _logger;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public SendEmailAlertOnProcessFailures(ILogger<SendEmailAlertOnProcessFailures> logger, IConfiguration configuration, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
            _configuration = configuration;
        }

        [Function(nameof(SendEmailAlertOnProcessFailures))]
        public async Task Run([QueueTrigger("failednotifications", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
            // Send email alert
            await _mailService.SendEmailAsync(
                _configuration["MailTo"],
                "Failed Notifications",
                message.MessageText
             );

        }
    }
}
