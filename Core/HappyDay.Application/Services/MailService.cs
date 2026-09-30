using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class MailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<MailService> _logger;

    public MailService(IConfiguration config, ILogger<MailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var host = _config["MailSettings:Host"];
        var port = _config.GetValue("MailSettings:Port", 587);
        var fromEmail = _config["MailSettings:FromEmail"];
        var fromName = _config["MailSettings:FromName"] ?? "Mutlu Günüm";
        var userName = _config["MailSettings:UserName"];
        var password = _config["MailSettings:Password"];

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Mail ayarları eksik. E-posta gönderilmeyecek. (MailSettings:*)");
            return;
        }

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(fromName, fromEmail ?? userName));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(userName, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
