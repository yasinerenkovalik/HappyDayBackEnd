using MailKit.Net.Smtp;
using MimeKit;

public class MailService
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Mutlu Günüm", "yasinerenkovalik@gmail.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        email.Body = new TextPart("html") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("yasinerenkovalik@gmail.com", "gkpv gvko znzw oisz");
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}