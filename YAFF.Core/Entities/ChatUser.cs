using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Entities
{
    public class ChatUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}