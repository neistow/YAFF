using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.Common;
using YAFF.Api.DTO;
using YAFF.Api.DTO.Post;
using YAFF.Api.Extensions;
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
        public async Task<IActionResult> GetPosts([FromQuery] PostQueryDto request)
        {
            var result = await Mediator.Send(new GetPostsQuery
            {
                IncludeTags = request.IncludeTags,
                InclusionMode = request.InclusionMode,
                ExcludeTags = request.ExcludeTags,
                ExclusionMode = request.ExclusionMode,
                Page = request.Page,
                PageSize = request.PageSize
            });
            return !result.Succeeded
                ? NotFound(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [AllowAnonymous]
        [HttpGet("{id:min(1)}")]
        public async Task<IActionResult> GetPost([FromRoute] int id)
        {
            var result = await Mediator.Send(new GetPostQuery {Id = id});
            return !result.Succeeded
                ? NotFound(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [AllowAnonymous]
        [HttpGet("{postId:min(1)}/comments")]
        public async Task<IActionResult> Comments([FromRoute] int postId, [FromQuery] PaginationDto request)
        {
            var result = await Mediator.Send(new GetCommentsOfPostQuery
            {
                PostId = postId,
                Page = request.Page,
                PageSize = request.PageSize
            });

            return !result.Succeeded
                ? NotFound(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto createPost)
        {
            var result = await Mediator.Send(new CreatePostRequest
            {
                Title = createPost.Title,
                Body = createPost.Body,
                AuthorId = CurrentUserId!.Value,
                Tags = createPost.Tags,
                PreviewBody = createPost.PreviewBody,
                PreviewImage = new FileAdapter(createPost.PreviewImage)
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : CreatedAtAction(nameof(GetPost), new {id = result.Data.Id}, result.ToApiResponse());
        }

        [HttpPut]
        public async Task<IActionResult> EditPost([FromForm] EditPostDto editPost)
        {
            var result = await Mediator.Send(new EditPostCommand
            {
                Id = editPost.Id,
                Title = editPost.Title,
                Body = editPost.Body,
                Tags = editPost.Tags,
                EditorId = CurrentUserId!.Value,
                PreviewBody = editPost.PreviewBody,
                PreviewImage = editPost.PreviewImage == null ? null : new FileAdapter(editPost.PreviewImage)
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            var result = await Mediator.Send(new DeletePostCommand
            {
                PostId = id,
                UserId = CurrentUserId!.Value
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok();
        }

        [HttpPost("{postId:min(1)}/addLike")]
        public async Task<IActionResult> AddLikeToPost([FromRoute] int postId)
        {
            var result = await Mediator.Send(new AddLikeToPostRequest
            {
                PostId = postId,
                UserId = CurrentUserId!.Value
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [HttpPost("{postId:min(1)}/removeLike")]
        public async Task<IActionResult> RemoveLikeFromPost([FromRoute] int postId)
        {
            var result = await Mediator.Send(new RemoveLikeFromPostRequest
            {
                PostId = postId,
                UserId = CurrentUserId!.Value
            });
            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }
    }
}