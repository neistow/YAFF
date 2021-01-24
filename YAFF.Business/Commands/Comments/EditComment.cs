using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
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
        private readonly IMapper _mapper;

        public EditCommentRequestHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<CommentDto>> Handle(EditCommentRequest request,
            CancellationToken cancellationToken)
        {
            var comment = await _forumDbContext.Comments
                .SingleOrDefaultAsync(c => c.Id == request.CommentId && c.AuthorId == request.AuthorId);
            if (comment == null)
            {
                return Result<CommentDto>.Failure(nameof(request.CommentId),
                    "Comment doesn't exist or you can't edit it");
            }

            comment.Body = request.Body;
            comment.DateAdded = DateTime.UtcNow;

            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<CommentDto>(comment);
            return Result<CommentDto>.Success(result);
        }
    }
}