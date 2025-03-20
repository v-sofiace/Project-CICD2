using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using Microsoft.Extensions.Logging;


namespace func_WarehouseBoxSys.Services
{

    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;

        private readonly IConfiguration _config;
        public MailService(IConfiguration config, ILogger<MailService> logger)
        {
            _config = config;
            _logger = logger;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpLogPath = _config["SmtpLogPath"];
                var email = new MimeMessage();
                // Log Display Name and MailFrom
                var displayName = _config["DisplayName"];
                var mailFrom = _config["MailFrom"];
                var smtpHost = _config["SmtpHost"];
                var smtpPort = _config["SmtpPort"];
                var smtpHostPwd = _config["Password"];

                _logger.LogInformation($"DisplayName: {displayName}, MailFrom: {mailFrom}, MailTo: {toEmail}");
                _logger.LogInformation($"SMTP Host: {smtpHost}, SMTP Port: {smtpPort}");
                _logger.LogInformation($"SMTP Host Pwd: {smtpHostPwd}");


                email.From.Add(new MailboxAddress(displayName, mailFrom));
                email.To.Add(new MailboxAddress("App Dev Team", toEmail));
                email.Subject = subject;
                body = "Failed Shippo notification";
                var builder = new BodyBuilder { HtmlBody = body };
                email.Body = builder.ToMessageBody();

                using (var smtp = new MailKit.Net.Smtp.SmtpClient(new ProtocolLogger(smtpLogPath)))
                {
                    smtp.Connect(smtpHost, int.Parse(smtpPort), MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate(_config["MailFrom"], _config["Password"]);
                    smtp.Send(email);
                    _logger.LogInformation("Email sent successfully");
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                _logger.LogError($"An error occurred while sending email: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");

            }
        }
    }
}
