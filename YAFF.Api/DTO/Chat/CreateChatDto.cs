using System.Collections.Generic;

namespace YAFF.Api.DTO.Chat
{
    public class CreateChatDto
    {
        public bool IsPrivate { get; set; }
        public IEnumerable<int> ChatUsers { get; set; }
    }
}