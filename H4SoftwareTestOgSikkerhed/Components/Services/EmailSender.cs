using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using H4SoftwareTestOgSikkerhed.Data;
using Microsoft.Extensions.Configuration;

public class EmailSender : ICustomEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IConfiguration configuration)
    {
        _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
    }

    // Send confirmation email
    public async Task SendEmailConfirmationAsync(ApplicationUser user, string callbackUrl)
    {
        var subject = "Confirm your email";
        var htmlMessage = $"Please confirm your email by clicking <a href='{callbackUrl}'>here</a>";
        await SendEmailAsync(user, subject, htmlMessage);
    }

    // Send password reset email
    public async Task SendPasswordResetAsync(ApplicationUser user, string callbackUrl)
    {
        var subject = "Reset your password";
        var htmlMessage = $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>";
        await SendEmailAsync(user, subject, htmlMessage);
    }

    // Send password reset email
    public async Task SendWeatherEmailAsync(ApplicationUser user, string subject, string Weatherdata)
    {
        await SendEmailAsync(user, subject, Weatherdata);
    }

    // Send general email
    public async Task SendGeneralEmailAsync(ApplicationUser user, string subject, string message)
    {
        await SendEmailAsync(user, subject, message);
    }

    // A method to send emails (general method used by all types of emails)
    private async Task SendEmailAsync(ApplicationUser user, string subject, string htmlMessage)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromAddress, _emailSettings.FromName),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(user.Email);

        using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SmtpPassword);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
