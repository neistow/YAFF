using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.Extensions;
using YAFF.Business.Queries.Posts;

namespace YAFF.Api.Controllers
{
    public class PostsController : ApiControllerBase
    {
        public PostsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await Mediator.Send(new GetPostsQuery {Page = page, PageSize = pageSize});
            return !result.Succeeded
                ? (IActionResult) BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }
    }
}