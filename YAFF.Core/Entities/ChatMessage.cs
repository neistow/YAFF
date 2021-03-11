using System;
using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public DateTime DateSent { get; set; }
        public string Text { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }
        
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}