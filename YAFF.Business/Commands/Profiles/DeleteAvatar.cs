using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.Interfaces;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Commands.Profiles
{
    public class DeleteAvatarCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
    }

    public class DeleteAvatarCommandHandler : IRequestHandler<DeleteAvatarCommand, Result<object>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IPhotoStorage _photoStorage;

        public DeleteAvatarCommandHandler(ForumDbContext forumDbContext, IPhotoStorage photoStorage)
        {
            _forumDbContext = forumDbContext;
            _photoStorage = photoStorage;
        }

        public async Task<Result<object>> Handle(DeleteAvatarCommand request, CancellationToken cancellationToken)
        {
            var profile = await _forumDbContext.Profiles
                .IncludeUser()
                .SingleAsync(p => p.UserId == request.UserId);

            var avatar = profile.Avatar;
            if (avatar == null)
            {
                return Result<object>.Failure(string.Empty, "Avatar doesn't exist");
            }

            _forumDbContext.Remove(avatar);
            await _photoStorage.DeletePhotoAsync(avatar.FileName);
            
            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}