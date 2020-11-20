using System;

namespace YAFF.Core.DTO
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? BanLiftDate { get; set; }
        public string Avatar { get; set; }
    }
}