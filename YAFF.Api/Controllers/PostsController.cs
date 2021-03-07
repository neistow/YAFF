using System.Collections.Generic;
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
using YAFF.Core.Common;
using YAFF.Core.DTO;
using PostDto = YAFF.Api.DTO.Post.PostDto;
using PostDtoCore = YAFF.Core.DTO.PostDto;

namespace YAFF.Api.Controllers
{
    public class PostsController : ApiControllerBase
    {
        public PostsController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<PostListItemDto>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 404)]
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
        
        [ProducesResponseType(typeof(PostDtoCore), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> GetPost([FromRoute] int id)
        {
            var result = await Mediator.Send(new GetPostQuery {Id = id});
            return !result.Succeeded
                ? NotFound(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [AllowAnonymous]
        [HttpGet("user/{userId:min(1)}")]
        [ProducesResponseType(typeof(PagedList<PostListItemDto>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 404)]
        public async Task<IActionResult> GetUserPosts([FromRoute] int userId, [FromQuery] PaginationDto pagination)
        {
            var result = await Mediator.Send(new GetUserPostsQuery
            {
                UserId = userId,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : NotFound(result.ToApiError());
        }


        [AllowAnonymous]
        [HttpGet("{postId:min(1)}/comments")]
        
        [ProducesResponseType(typeof(PagedList<CommentDto>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 404)]
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
        
        [ProducesResponseType(typeof(PostDtoCore),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> CreatePost([FromForm] PostDto dto)
        {
            var result = await Mediator.Send(new CreatePostRequest
            {
                Title = dto.Title,
                Body = dto.Body,
                AuthorId = CurrentUserId!.Value,
                Tags = dto.Tags,
                PreviewBody = dto.PreviewBody,
                PreviewImage = new FileAdapter(dto.PreviewImage)
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : CreatedAtAction(nameof(GetPost), new {id = result.Data.Id}, result.ToApiResponse());
        }

        [HttpPut("{id:min(1)}")]
        
        [ProducesResponseType(typeof(PostDtoCore),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromForm] PostDto dto)
        {
            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = id,
                Title = dto.Title,
                Body = dto.Body,
                Tags = dto.Tags,
                EditorId = CurrentUserId!.Value,
                PreviewBody = dto.PreviewBody,
                PreviewImage = dto.PreviewImage == null ? null : new FileAdapter(dto.PreviewImage)
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
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

        
        [HttpPost("{postId:min(1)}/likes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
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

        
        [HttpDelete("{postId:min(1)}/likes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
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