using System;

namespace YAFF.Api.DTO.Comment
{
    public record UpdateCommentDto
    {
        public Guid Id { get; init; }
        public string Body { get; init; }
    }
}