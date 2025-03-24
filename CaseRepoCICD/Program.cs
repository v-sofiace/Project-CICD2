/* using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run(); */

using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Queues;
using func_WarehouseBoxSys.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Azure.Identity;

var builder = FunctionsApplication.CreateBuilder(args);

string azStorageQueueConn = builder.Configuration["ShippoNotificationQueueConn"];

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//builder.Services.AddApplicationInsightsTelemetryWorkerService(options =>
//{
//    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
//});

// Configure logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddApplicationInsights();
});

//// Register Azure Queue Storage
builder.Services.AddAzureClients(clients =>
{
    clients.AddQueueServiceClient(azStorageQueueConn);

});

// Register AzureQueueHelper
builder.Services.AddScoped<IAzureQueueHelper, AzureQueueHelper>();

// Register HttpClient
builder.Services.AddHttpClient();
builder.Services.AddTransient<IMailService, MailService>();

builder.Build().Run();
