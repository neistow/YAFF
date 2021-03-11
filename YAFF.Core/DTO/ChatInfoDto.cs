using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public class ChatInfoDto
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Title { get; set; }

        public List<ChatUserDto> Users { get; set; }
    }
}