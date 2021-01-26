using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Commands.Profiles
{
    public class EditProfileCommand : IRequest<Result<UserProfileDto>>
    {
        public int UserId { get; set; }
        public string UserStatus { get; set; }
        public string Bio { get; set; }
        public UserProfileType ProfileType { get; set; }
    }

    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Result<UserProfileDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public EditProfileCommandHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<UserProfileDto>> Handle(EditProfileCommand request,
            CancellationToken cancellationToken)
        {
            var profile = await _forumDbContext.Profiles
                .IncludeUser()
                .SingleOrDefaultAsync(p => p.UserId == request.UserId);

            if (profile == null)
            {
                return Result<UserProfileDto>.Failure(nameof(profile.Id),
                    "Profile doesn't exist or you can't edit it");
            }

            profile.UserStatus = request.UserStatus;
            profile.Bio = request.Bio;
            profile.ProfileType = request.ProfileType;

            await _forumDbContext.SaveChangesAsync();

            return Result<UserProfileDto>.Success(_mapper.Map<UserProfileDto>(profile));
        }
    }
}