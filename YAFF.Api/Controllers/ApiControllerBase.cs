using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YAFF.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;

        protected ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected int? CurrentUserId
        {
            get
            {
                var claim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "Id");
                return claim == null ? null : int.Parse(claim.Value);
            }
        }
    }
}