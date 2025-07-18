using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgClientUI.Messages
{
    public class ChatMessage
    {
        public DateTime timestamp { get; set; }
        public int author { get; set; }
        public string channel { get; set; }
        public string content { get; set; }
    }
}
