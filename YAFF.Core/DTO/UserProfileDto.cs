using System;
using YAFF.Core.Entities;

namespace YAFF.Core.DTO
{
    public record UserProfileDto
    {
        public int Id { get; init; }

        public string UserName { get; init; }
        public string UserStatus { get; init; }
        public string Bio { get; init; }
        public DateTime RegistrationDate { get; init; }
        public DateTime? LastLoginDate { get; init; }
        public UserProfileType ProfileType { get; init; }

        public string Avatar { get; init; }
    }
}