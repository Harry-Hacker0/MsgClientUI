using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgClientUI.Messages
{
    public class InfrastructureMessage
    {
        public DateTime timestamp { get; set; }
        public string infrastructureType { get; set; }
        public string infrastructureDetails { get; set; }
    }
}
