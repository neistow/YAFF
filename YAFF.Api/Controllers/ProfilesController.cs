using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.Common;
using YAFF.Api.DTO.Profile;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Profiles;
using YAFF.Business.Queries.Profiles;
using YAFF.Core.DTO;

namespace YAFF.Api.Controllers
{
    public class ProfilesController : ApiControllerBase
    {
        public ProfilesController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet("{userId:min(1)}")]
        [ProducesResponseType(typeof(UserProfileDto),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> GetUsersProfile(int userId)
        {
            var result = await Mediator.Send(new GetProfileQuery
            {
                UserId = userId,
                CurrentUserId = CurrentUserId
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserProfileDto),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto profileDto)
        {
            var result = await Mediator.Send(new EditProfileCommand
            {
                Bio = profileDto.Bio,
                UserStatus = profileDto.UserStatus,
                ProfileType = profileDto.ProfileType,
                UserId = CurrentUserId!.Value
            });
            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [HttpPut("avatar")]
        [ProducesResponseType(typeof(UserProfileDto),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile avatar)
        {
            var result = await Mediator.Send(new EditAvatarCommand
            {
                Avatar = new FileAdapter(avatar),
                UserId = CurrentUserId!.Value
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [HttpDelete("avatar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> DeleteAvatar()
        {
            var result = await Mediator.Send(new DeleteAvatarCommand
            {
                UserId = CurrentUserId!.Value
            });

            return result.Succeeded
                ? Ok()
                : BadRequest(result.ToApiError());
        }
    }
}