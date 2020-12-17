using System;

namespace YAFF.Core.DTO
{
    public class AuthorDto
    {
        public Guid Id { get; init; }
        public string NickName { get; init; }
        public string Avatar { get; init; }
    }
}