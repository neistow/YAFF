using System.Collections.Generic;

namespace YAFF.Core.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }

        public ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}