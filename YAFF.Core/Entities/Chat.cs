using System.Collections.Generic;

namespace YAFF.Core.Entities
{
    public abstract class Chat
    {
        public int Id { get; set; }

        public ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}