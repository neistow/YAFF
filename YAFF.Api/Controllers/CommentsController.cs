﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO.Comment;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Comments;

namespace YAFF.Api.Controllers
{
    public class CommentsController : ApiControllerBase
    {
        public CommentsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto request)
        {
            var result = await Mediator.Send(new AddCommentRequest
            {
                PostId = request.PostId,
                Body = request.Body,
                ReplyTo = request.ReplyTo,
                AuthorId = CurrentUserId!.Value
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [HttpPut("{id:min(1)}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
        {
            var result = await Mediator.Send(new EditCommentRequest
            {
                CommentId = id,
                Body = request.Body,
                AuthorId = CurrentUserId!.Value
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var result = await Mediator.Send(new DeleteCommentRequest
            {
                CommentId = id,
                UserId = CurrentUserId!.Value
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }
    }
}