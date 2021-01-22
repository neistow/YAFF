using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;
using YAFF.Data;

namespace YAFF.Business.Commands.Comments
{
    public class AddCommentRequest : IRequest<Result<CommentDto>>
    {
        public int PostId { get; init; }
        public string Body { get; init; }
        public int? ReplyTo { get; init; }
        public int AuthorId { get; init; }
    }

    public class AddCommentRequestHandler : IRequestHandler<AddCommentRequest, Result<CommentDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AddCommentRequestHandler(ForumDbContext forumDbContext, UserManager<User> userManager, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<CommentDto>> Handle(AddCommentRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.AuthorId.ToString());
            if (user == null)
            {
                return Result<CommentDto>.Failure();
            }

            if (user.IsBanned)
            {
                return Result<CommentDto>.Failure(string.Empty, "You are banned");
            }

            var post = await _forumDbContext.Posts.FindAsync(request.PostId);
            if (post == null)
            {
                return Result<CommentDto>.Failure(nameof(request.PostId), "Post doesn't exist");
            }

            if (request.ReplyTo.HasValue)
            {
                var replyComment = await _forumDbContext.Comments.FindAsync(request.ReplyTo.Value);
                if (replyComment == null)
                {
                    return Result<CommentDto>.Failure(nameof(request.ReplyTo), "Comment doesn't exist");
                }
            }

            var comment = new Comment
            {
                Body = request.Body,
                PostId = request.PostId,
                ReplyToId = request.ReplyTo,
                AuthorId = request.AuthorId,
                DateAdded = DateTime.UtcNow
            };

            await _forumDbContext.Comments.AddAsync(comment);
            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<CommentDto>(comment);
            return Result<CommentDto>.Success(result);
        }
    }
}