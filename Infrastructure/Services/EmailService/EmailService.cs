using Domain.DTOs.EmailDto;
using Domain.DTOs.EmailDTOs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services.EmailService;

public class EmailService(EmailConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{

    public async Task SendEmail(EmailMessageDto message, TextFormat format)
    {
        logger.LogInformation("Starting method {SendEmail} in time {DateTime}", "SendEmail", DateTimeOffset.UtcNow);
        var emailMessage = CreateEmailMessage(message, format);
        await SendAsync(emailMessage);
        logger.LogInformation("Finished method {SendEmail} in time {DateTime}", "SendEmail", DateTimeOffset.UtcNow);
    }


    private MimeMessage CreateEmailMessage(EmailMessageDto message, TextFormat format)
    {
        logger.LogInformation("Starting method {CreateEmailMessage} in time {DateTime}", "CreateEmailMessage",
            DateTimeOffset.UtcNow);
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("mail", configuration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(format) { Text = message.Content };
        logger.LogInformation("Finished method {CreateEmailMessage} in time {DateTime}", "CreateEmailMessage",
            DateTimeOffset.UtcNow);
        return emailMessage;
    }



    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            logger.LogInformation("Starting method {SendAsync} in time {DateTime}", "SendAsync", DateTimeOffset.UtcNow);
            await client.ConnectAsync(configuration.SmtpServer, configuration.Port, true);
            client.AuthenticationMechanisms.Remove("OAUTH2");
            await client.AuthenticateAsync(configuration.UserName, configuration.Password);

            await client.SendAsync(mailMessage);
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
            logger.LogInformation("Finished method {SendAsync} in time {DateTime}", "SendAsync", DateTimeOffset.UtcNow);
        }
    }

}