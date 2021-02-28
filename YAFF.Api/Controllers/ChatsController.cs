using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.DTO.Chat;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Chats;
using YAFF.Business.Queries.Chat;
using YAFF.Core.Common;
using YAFF.Core.DTO;

namespace YAFF.Api.Controllers
{
    public class ChatsController : ApiControllerBase
    {
        public ChatsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{chatId}/messages")]
        [ProducesResponseType(typeof(PagedList<ChatMessageDto>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> GetChatMessages([FromRoute] int chatId,
            [FromQuery] PaginationDto paginationDto)
        {
            var result = await Mediator.Send(new GetChatMessagesQuery
            {
                Page = paginationDto.Page,
                PageSize = paginationDto.PageSize,
                ChatId = chatId,
                UserId = CurrentUserId!.Value
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [HttpGet("{chatId}")]
        [ProducesResponseType(typeof(ChatInfoDto),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> GetChatInfo([FromRoute] int chatId)
        {
            var result = await Mediator.Send(new GetChatInfoRequest
            {
                ChatId = chatId,
                UserId = CurrentUserId!.Value
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ChatInfoDto),200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
        {
            var result = await Mediator.Send(new CreateChatCommand
            {
                CreatorId = CurrentUserId!.Value,
                IsPrivate = dto.IsPrivate,
                Users = dto.ChatUsers
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }
    }
}