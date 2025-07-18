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
    public class UsersList : ObservableCollection<UserItem>
    {

        public UsersList() : base() 
        { 
        }
    }

    public class UserItem
    {
        private DateTime created;
        private string authorID;
        private string name;

        public UserItem(string messageAuthor)
        {
            this.authorID = messageAuthor;
        }

        public UserItem(InfrastructureMessage recievedMessage)
        {
            string[] details = recievedMessage.infrastructureDetails.Split(",");
            this.Created = recievedMessage.timestamp;
            this.AuthorID = details[0];
            this.Name = details[1];
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public string AuthorID
        {
            get { return authorID; }
            set { authorID = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
