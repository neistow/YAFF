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

namespace YAFF.Business.Queries.Profiles
{
    public class GetProfileQuery : IRequest<Result<UserProfileDto>>
    {
        public int? CurrentUserId { get; set; }
        public int UserId { get; set; }
    }

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<UserProfileDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetProfileQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _forumDbContext.Profiles
                .IncludeUser()
                .AsNoTracking()
                .SingleOrDefaultAsync(up => up.UserId == request.UserId);
            if (profile == null)
            {
                return Result<UserProfileDto>.Failure(string.Empty, "Profile doesn't exist");
            }

            if (profile.ProfileType == UserProfileType.Private && request.CurrentUserId != profile.UserId)
            {
                return Result<UserProfileDto>.Failure(nameof(profile.ProfileType), "This profile is private");
            }

            var result = _mapper.Map<UserProfileDto>(profile);
            return Result<UserProfileDto>.Success(result);
        }
    }
}