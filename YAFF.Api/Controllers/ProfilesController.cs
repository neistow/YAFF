using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO.Profile;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Profiles;
using YAFF.Business.Queries.Profiles;

namespace YAFF.Api.Controllers
{
    public class ProfilesController : ApiControllerBase
    {
        public ProfilesController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet("{profileId:min(1)}")]
        public async Task<IActionResult> GetProfile(int profileId)
        {
            var result = await Mediator.Send(new GetProfileQuery
            {
                ProfileId = profileId,
                UserId = CurrentUserId
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [HttpPut("{profileId:min(1)}")]
        public async Task<IActionResult> EditProfile(int profileId, EditProfileDto profileDto)
        {
            var result = await Mediator.Send(new EditProfileCommand
            {
                ProfileId = profileId,
                Bio = profileDto.Bio,
                UserStatus = profileDto.UserStatus,
                ProfileType = profileDto.ProfileType,
                UserId = CurrentUserId!.Value
            });
            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }
    }
}