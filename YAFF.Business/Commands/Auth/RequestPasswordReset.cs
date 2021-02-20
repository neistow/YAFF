using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.Entities.Identity;
using YAFF.Core.Interfaces;

namespace YAFF.Business.Commands.Auth
{
    public class RequestPasswordResetCommand : IRequest<Result<object>>
    {
        public string Email { get; set; }
    }

    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Result<object>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public RequestPasswordResetCommandHandler(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<Result<object>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<object>.Failure(nameof(request.Email), "Incorrect email");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            await _emailSender.SendEmail(user.Email, "Password reset",
                $"You have requested a password reset for YAFF.\nResetCode: {base64Token}\nIf it wasn't you please immideately contact support");

            return Result<object>.Success();
        }
    }
}