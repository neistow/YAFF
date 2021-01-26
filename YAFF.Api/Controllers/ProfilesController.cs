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

namespace YAFF.Api.Controllers
{
    public class ProfilesController : ApiControllerBase
    {
        public ProfilesController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet("{userId:min(1)}")]
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
        public async Task<IActionResult> EditProfile(EditProfileDto profileDto)
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

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile avatar)
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