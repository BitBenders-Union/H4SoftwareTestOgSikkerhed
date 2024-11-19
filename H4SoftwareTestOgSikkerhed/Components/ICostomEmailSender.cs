using H4SoftwareTestOgSikkerhed.Data;

public interface ICustomEmailSender
{
    Task SendEmailConfirmationAsync(ApplicationUser user, string callbackUrl);
    Task SendPasswordResetAsync(ApplicationUser user, string callbackUrl);
    Task SendGeneralEmailAsync(ApplicationUser user, string subject, string message);
}
