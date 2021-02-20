using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Business.Notifications;
using YAFF.Core.Common;
using YAFF.Core.Entities.Identity;

namespace YAFF.Business.Commands.Auth
{
    public class ChangePasswordCommand : IRequest<Result<object>>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public int UserId { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<object>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ChangePasswordCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result<object>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
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