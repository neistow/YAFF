using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Business.Notifications;
using YAFF.Core.Common;
using YAFF.Core.Entities.Identity;

namespace YAFF.Business.Commands.Auth
{
    public class ResetPasswordCommand : IRequest<Result<object>>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<object>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ResetPasswordCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result<object>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<object>.Failure("", "User doesn't exist");
            }

            var token = Encoding.UTF8.GetString(Convert.FromBase64String(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result<object>.Failure("", error.Description);
            }

            await _mediator.Publish(new PasswordChangedNotification
            {
                User = user
            });

            return Result<object>.Success();
        }
    }
}