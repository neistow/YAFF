﻿using System;

namespace YAFF.Core.DTO
{
    public record UserInfo
    {
        public Guid Id { get; init; }
        public string NickName { get; init; }
        public DateTime RegistrationDate { get; init; }
        public string Email { get; init; }
        public bool EmailConfirmed { get; init; }
        public bool IsBanned { get; init; }
        public DateTime? BanLiftDate { get; init; }
        public string Avatar { get; init; }
    }
}