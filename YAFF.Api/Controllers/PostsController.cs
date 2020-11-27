using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.Extensions;
using YAFF.Business.Queries.Posts;

namespace YAFF.Api.Controllers
{
    public class PostsController : ApiControllerBase
    {
        public PostsController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PaginationDto request)
        {
            var result = await Mediator.Send(new GetPostsQuery {Page = request.Page, PageSize = request.PageSize});
            return !result.Succeeded
                ? (IActionResult) BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetPostQuery {Id = id});
            return !result.Succeeded
                ? (IActionResult) NotFound(result.ToApiError(404))
                : Ok(result.ToApiResponse(200));
        }
    }
}