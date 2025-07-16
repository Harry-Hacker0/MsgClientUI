using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsgClientUI.Messages;

namespace MsgClientUI
{
    public class UsersList : ObservableCollection<UserItem>
    {

        public UsersList() : base() 
        { 
            DateTime dateTime = DateTime.Now;
            Add(new UserItem(1337));
        }
    }

    public class UserItem
    {
        private DateTime created;
        private int author;
        private string content;

        public UserItem(int messageAuthor)
        {
            this.author = messageAuthor;
        }

        private UserItem(ChatMessage recievedMessage)
        {
            this.author = recievedMessage.author;
        }

        public int Author
        {
            get { return author; }
            set { author = value; }
        }
    }
}
