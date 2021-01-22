using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities.Identity;
using YAFF.Data;

namespace YAFF.Business.Commands.Comments
{
    public class EditCommentRequest : IRequest<Result<CommentDto>>
    {
        public int CommentId { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }
    }

    public class EditCommentRequestHandler : IRequestHandler<EditCommentRequest, Result<CommentDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public EditCommentRequestHandler(ForumDbContext forumDbContext, UserManager<User> userManager, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<CommentDto>> Handle(EditCommentRequest request,
            CancellationToken cancellationToken)
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

            var comment = await _forumDbContext.Comments.FindAsync(request.CommentId);
            if (comment == null)
            {
                return Result<CommentDto>.Failure(nameof(request.CommentId), "Comment doesn't exist");
            }

            if (comment.AuthorId != user.Id)
            {
                return Result<CommentDto>.Failure();
            }

            comment.Body = request.Body;
            comment.DateAdded = DateTime.UtcNow;

            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<CommentDto>(comment);
            return Result<CommentDto>.Success(result);
        }
    }
}