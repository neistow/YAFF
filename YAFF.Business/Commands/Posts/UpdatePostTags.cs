using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Entities;
using YAFF.Data;

namespace YAFF.Business.Commands.Posts
{
    public class UpdatePostTagsCommand : IRequest
    {
        public IEnumerable<string> Tags { get; init; }
        public Post Post { get; init; }
    }

    public class UpdatePostTagsCommandHandler : AsyncRequestHandler<UpdatePostTagsCommand>
    {
        private readonly ForumDbContext _forumDbContext;

        public UpdatePostTagsCommandHandler(ForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        protected override async Task Handle(UpdatePostTagsCommand request, CancellationToken cancellationToken)
        {
            _forumDbContext.PostTags.RemoveRange(request.Post.PostTags);

            foreach (var tagName in request.Tags)
            {
                var tagInDb = await _forumDbContext.Tags
                    .SingleOrDefaultAsync(t => t.Name == tagName.ToLowerInvariant());
                if (tagInDb != null)
                {
                    request.Post.PostTags.Add(new PostTag {TagId = tagInDb.Id});
                }
                else
                {
                    var tag = new Tag {Name = tagName.ToLowerInvariant()};
                    await _forumDbContext.Tags.AddAsync(tag);

                    request.Post.PostTags.Add(new PostTag {Tag = tag});
                }
            }
        }
    }
}