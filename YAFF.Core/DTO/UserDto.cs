using System;

namespace YAFF.Core.DTO
{
    public record UserDto
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public DateTime RegistrationDate { get; init; }
        public string Email { get; init; }
        public bool EmailConfirmed { get; init; }
        public bool IsBanned { get; init; }
        public DateTime? BanLiftDate { get; init; }
        public string Avatar { get; init; }
    }
}