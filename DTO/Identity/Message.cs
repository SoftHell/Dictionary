using System.Collections.Generic;

namespace DTO.Identity
{
    public class Message
    {
        public Message()
        {
            
        }

        public Message(params string[] messages)
        {
            Messages = messages;
        }
        public IList<string> Messages { get; set; } = new List<string>();
    }
}