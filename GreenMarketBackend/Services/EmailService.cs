using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

        var client = new System.Net.Mail.SmtpClient(smtpSettings.Server) // Fully qualify here
        {
            Port = smtpSettings.Port,
            Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings.SenderEmail, smtpSettings.SenderName),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}

public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
