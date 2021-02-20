using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using YAFF.Core.Entities.Identity;
using YAFF.Core.Interfaces;

namespace YAFF.Business.Notifications
{
    public class PasswordChangedNotification : INotification
    {
        public User User { get; set; }
    }

    public class EmailNotificationHandler : INotificationHandler<PasswordChangedNotification>
    {
        private readonly IEmailSender _emailSender;

        public EmailNotificationHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Handle(PasswordChangedNotification notification, CancellationToken cancellationToken)
        {
            await _emailSender.SendEmail(notification.User.Email, "Password Changed",
                "Dear user, your password for YAFF <b>was changed</b>.\nIf it wasn't you please immediately contact support.", true);
        }
    }
}