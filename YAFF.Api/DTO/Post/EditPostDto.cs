using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace YAFF.Api.DTO.Post
{
    public class EditPostDto
    {
        public int Id { get; set; }
        public string Title { get; init; }
        public string Body { get; init; }
        public List<string> Tags { get; init; } = new List<string>();

        public string PreviewBody { get; init; }
        public IFormFile PreviewImage { get; init; }
    }
}