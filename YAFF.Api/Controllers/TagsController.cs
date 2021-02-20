using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.Extensions;
using YAFF.Business.Queries.Tags;

namespace YAFF.Api.Controllers
{
    public class TagsController : ApiControllerBase
    {
        public TagsController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 404)]
        public async Task<IActionResult> GetTags([FromQuery] PaginationDto request)
        {
            var result = await Mediator.Send(new GetTagsRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            });
            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : NotFound(result.ToApiError());
        }
    }
}