using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Commands.Posts
{
    public class EditPostCommand : IRequest<Result<PostDto>>
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public int EditorId { get; set; }
    }

    public class EditPostCommandHandler : IRequestHandler<EditPostCommand, Result<PostDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public EditPostCommandHandler(ForumDbContext forumDbContext, UserManager<User> userManager, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.EditorId.ToString());
            if (user == null)
            {
                return Result<PostDto>.Failure();
            }

            if (user.IsBanned)
            {
                return Result<PostDto>.Failure(string.Empty, "You are banned.");
            }

            var post = await _forumDbContext.Posts
                .IncludeTags()
                .SingleOrDefaultAsync(p => p.Id == request.PostId && p.AuthorId == user.Id);
            if (post == null)
            {
                return Result<PostDto>.Failure(nameof(request.PostId), "Post doesn't exist or you can't edit it");
            }

            post.Title = request.Title;
            post.Body = request.Body;

            _forumDbContext.PostTags.RemoveRange(post.PostTags);

            foreach (var tagName in request.Tags)
            {
                var tagInDb = await _forumDbContext.Tags
                    .SingleOrDefaultAsync(t => t.Name == tagName.ToLowerInvariant());
                if (tagInDb != null)
                {
                    post.PostTags.Add(new PostTag {TagId = tagInDb.Id});
                }
                else
                {
                    var tag = new Tag {Name = tagName.ToLowerInvariant()};
                    await _forumDbContext.Tags.AddAsync(tag);

                    post.PostTags.Add(new PostTag {Tag = tag});
                }
            }

            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}