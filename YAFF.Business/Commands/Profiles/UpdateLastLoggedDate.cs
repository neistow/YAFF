using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Data;

namespace YAFF.Business.Commands.Profiles
{
    public class UpdateLastLoginDateCommand : IRequest
    {
        public int UserId { get; set; }
    }

    public class UpdateLastLoggedDateCommandHandler : AsyncRequestHandler<UpdateLastLoginDateCommand>
    {
        private readonly ForumDbContext _forumDbContext;

        public UpdateLastLoggedDateCommandHandler(ForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        protected override async Task Handle(UpdateLastLoginDateCommand request, CancellationToken cancellationToken)
        {
            var profile = await _forumDbContext.Profiles.SingleAsync(p => p.UserId == request.UserId);
            profile.LastLoginDate = DateTime.UtcNow;
            await _forumDbContext.SaveChangesAsync();
        }
    }
}