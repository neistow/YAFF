using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Comments;

namespace YAFF.Api.Controllers
{
    public class CommentsController : ApiControllerBase
    {
        public CommentsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> AddComment([FromRoute] Guid postId, [FromBody] PostCommentDto request)
        {
            var result = await Mediator.Send(new AddCommentRequest
            {
                PostId = postId,
                Body = request.Body,
                ReplyTo = request.ReplyTo,
                AuthorId = CurrentUserId
            });
            return !result.Succeeded
                ? (IActionResult) BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }
    }
}