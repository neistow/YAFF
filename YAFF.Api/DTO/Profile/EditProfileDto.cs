﻿using YAFF.Core.Entities;

namespace YAFF.Api.DTO.Profile
{
    public record EditProfileDto
    {
        public string Bio { get; init; }
        public string UserStatus { get; init; }
        public UserProfileType ProfileType { get; init; }
    }
}