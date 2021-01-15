using System;
using System.Collections.Generic;

namespace YAFF.Api.DTO.Post
{
    public record PostDto
    {
        public string Title { get; init; }
        public string Body { get; init; }
        public List<Guid> Tags { get; init; }
    }
}