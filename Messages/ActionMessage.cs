using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgClientUI.Messages
{
    internal class ActionMessage
    {
        public DateTime timestamp { get; set; }
        public string actionType { get; set; }
        public string actionDetails { get; set; }
    }
}
