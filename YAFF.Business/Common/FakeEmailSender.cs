using System.Threading.Tasks;
using YAFF.Core.Interfaces;

namespace YAFF.Business.Common
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmail(string to, string subject, string body, bool htmlBody = false)
        {
            return Task.CompletedTask;
        }
    }
}