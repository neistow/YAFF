using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.DTO.Post;
using YAFF.Api.Extensions;
using YAFF.Api.Helpers;
using YAFF.Business.Commands.Likes;
using YAFF.Business.Commands.Posts;
using YAFF.Business.Queries.Comments;
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
            var result = await Mediator.Send(new GetPostsQuery
            {
                Page = request.Page,
                PageSize = request.PageSize
            });
            return !result.Succeeded
                ? NotFound(result.ToApiError(404))
                : Ok(result.ToApiResponse(200));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetPostQuery {Id = id});
            return !result.Succeeded
                ? NotFound(result.ToApiError(404))
                : Ok(result.ToApiResponse(200));
        }

        [AllowAnonymous]
        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> Comments([FromRoute] Guid postId, [FromQuery] PaginationDto request)
        {
            var result = await Mediator.Send(new GetCommentsOfPostRequest
            {
                PostId = postId,
                Page = request.Page,
                PageSize = request.PageSize
            });

            return !result.Succeeded
                ? NotFound(result.ToApiError(404))
                : Ok(result.ToApiResponse(200));
        }

        [EnableTransaction]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto post)
        {
            var result = await Mediator.Send(new CreatePostRequest
            {
                Title = post.Title,
                Body = post.Body,
                AuthorId = CurrentUserId,
                Tags = post.Tags
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError(400))
                : CreatedAtAction(nameof(GetPost), new {id = result.Data.Id}, result.ToApiResponse(201));
        }


        [EnableTransaction]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost([FromRoute] Guid id, [FromBody] PostDto post)
        {
            var result = await Mediator.Send(new EditPostCommand
            {
                PostId = id,
                Title = post.Title,
                Body = post.Body,
                Tags = post.Tags,
                EditorId = CurrentUserId
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }

        [EnableTransaction]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new DeletePostCommand
            {
                PostId = id,
                UserId = CurrentUserId
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError(400))
                : Ok();
        }

        [EnableTransaction]
        [HttpPost("{postId}/addLike")]
        public async Task<IActionResult> AddLikeToPost([FromRoute] Guid postId)
        {
            var result = await Mediator.Send(new AddLikeToPostRequest
            {
                PostId = postId,
                UserId = CurrentUserId
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }

        [EnableTransaction]
        [HttpPost("{postId}/removeLike")]
        public async Task<IActionResult> RemoveLikeFromPost([FromRoute] Guid postId)
        {
            var result = await Mediator.Send(new RemoveLikeFromPostRequest
            {
                PostId = postId,
                UserId = CurrentUserId
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }
    }
}