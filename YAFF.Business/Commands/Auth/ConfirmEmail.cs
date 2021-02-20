using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.Entities.Identity;

namespace YAFF.Business.Commands.Auth
{
    public class ConfirmEmailCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<object>>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<object>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result<object>.Failure(nameof(request.UserId), "User doesn't exist");
            }

            var token = Encoding.UTF8.GetString(Convert.FromBase64String(request.Token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result<object>.Failure("", error.Description);
            }

            return Result<object>.Success();
        }
    }
}