using System.Collections.Generic;

namespace YAFF.Api.DTO.Chat
{
    public class CreateChatDto
    {
        public bool IsPrivate { get; set; }
        public string Title { get; set; }
        public List<int> ChatUsers { get; set; }
    }
}