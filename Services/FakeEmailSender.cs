using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
namespace PROG7311_PART_2.Services
{

    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just log or ignore email sending
            Console.WriteLine($"Email sent to {email}: {subject}");
            return Task.CompletedTask;
        }
    }

}
