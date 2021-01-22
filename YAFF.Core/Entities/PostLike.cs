using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Entities
{
    public record PostLike
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}