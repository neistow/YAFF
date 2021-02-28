using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using YAFF.Core.Common;

namespace YAFF.Api.Hubs.Common
{
    [Authorize]
    public abstract class HubBase<T> : Hub<T> where T : class
    {
        protected readonly IMediator Mediator;

        protected HubBase(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected void ThrowIfNotSucceeded<TResult>(Result<TResult> result)
        {
            if (!result.Succeeded)
            {
                throw new HubException(result.Message);
            }
        }
    }
}