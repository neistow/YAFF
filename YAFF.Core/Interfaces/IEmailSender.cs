using System.Threading.Tasks;

namespace YAFF.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string to, string subject, string body, bool htmlBody = false);
    }
}