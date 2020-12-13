using System;

namespace YAFF.Api.DTO.Comment
{
    public class UpdateCommentDto
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
    }
}