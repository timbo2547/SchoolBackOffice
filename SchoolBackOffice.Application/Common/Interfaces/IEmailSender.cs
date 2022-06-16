using System.Threading.Tasks;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}