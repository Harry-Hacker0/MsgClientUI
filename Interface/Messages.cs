using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MsgClientUI.Interface
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
        private int author;
        private string channel;
        private string content;


        public MessageItem(DateTime created, int author, string channel, string messageContent )
        {
            this.timestamp = created;
            this.author = author;
            this.channel = channel;
            this.content = messageContent;

            //MessageList.AddNewMessage( iD,  messageAuthor,  messageContent,  createdTime);
        }


        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public int Author
        {
            get { return author; }
            set { author = value; }
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
