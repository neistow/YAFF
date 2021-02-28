using System;

namespace YAFF.Core.DTO
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public DateTime DateSent { get; set; }
        public bool Seen { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
    }
}