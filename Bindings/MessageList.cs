using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MsgClientUI.Messages;

namespace MsgClientUI.Bindings
{
    public class MessageList : ObservableCollection<MessageItem>
    {

        public MessageList() : base() 
        {
        }
    }

    public class MessageItem
    {
        private DateTime timestamp;
        private int authorID;
        private string channel;
        private string content;


        public MessageItem(DateTime created, int author, string channel, string messageContent )
        {
            this.timestamp = created;
            this.authorID = author;
            this.channel = channel;
            this.content = messageContent;
        }

        public MessageItem(ChatMessage chatMessage)
        {
            this.timestamp = chatMessage.timestamp;
            this.authorID = chatMessage.author;
            this.channel = chatMessage.channel;
            this.content = chatMessage.content;
        }


        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public int AuthorID
        {
            get { return authorID; }
            set { authorID = value; }
        }

        public string Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
