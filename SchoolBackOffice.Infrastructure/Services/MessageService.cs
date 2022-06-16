using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolBackOffice.Application.Common.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace SchoolBackOffice.Infrastructure.Services
{
    public class MessageService
    {
        // This class is used by the application to send Email and SMS
        // when you turn on two-factor authentication in ASP.NET Identity.
        // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
        public class AuthMessageSender : IEmailSender
        {
            private readonly ILogger<AuthMessageSender> _logger;
            public AuthMessageSender(ILogger<AuthMessageSender> logger)
            {
                _logger = logger;
                Options = new AuthMessageSenderOptions();
            }
            
            private AuthMessageSenderOptions Options { get; }
            public async Task SendEmailAsync(string toEmail, string subject, string message)
            {
                if (string.IsNullOrEmpty(Options.SendGridKey))
                {
                    throw new Exception("Null SendGridKey");
                }
                await Execute(Options.SendGridKey, subject, message, toEmail);
            }
            
            private async Task Execute(string apiKey, string subject, string message, string toEmail)
            {
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("timbo2547@gmail.com", "Accel Micro School"),
                    Subject = subject,
                    PlainTextContent = message,
                    HtmlContent = message
                };
                msg.AddTo(new EmailAddress(toEmail));

                // Disable click tracking.
                // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
                msg.SetClickTracking(false, false);
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation(response.IsSuccessStatusCode 
                    ? $"Email to {toEmail} queued successfully!"
                    : $"Failure Email to {toEmail}");
            }

            // public Task SendSmsAsync(string number, string message)
            // {
            //     // Plug in your SMS service here to send a text message.
            //     return Task.FromResult(0);
            // }
        }
    }
}