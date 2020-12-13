using System;
using System.Collections.Generic;

namespace YAFF.Api.DTO.Post
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}