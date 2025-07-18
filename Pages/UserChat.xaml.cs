using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MsgClientUI.Bindings;
using MsgClientUI.Messages;

namespace MsgClientUI.Pages
{
    public partial class UserChat : Page
    {
        MainWindow window;
        MessageList msgList;
        string pageID;

        public UserChat()
        {
            window = (MainWindow)Application.Current.MainWindow;
            msgList = new MessageList();
            InitializeComponent();
            messageList.ItemsSource = msgList;
        }

        public void SetPageID(string someVal)
        {
            pageID = someVal;
        }

        public void AddChatMessage(MessageItem item)
        {
            msgList.Add(item);
            //if (messageList.Items.Count > 0) messageList.ScrollIntoView(messageList.Items[messageList.Items.Count - 1]);

            Debug.WriteLine($"User Added Count: {msgList.Count}");
        }

        private void Message_Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.author = window.user.ConnectionID;
                chatMessage.timestamp = DateTime.Now;
                chatMessage.content = messageTextbox.Text;
                chatMessage.channel = pageID;

                window.Connection.SendMessage("CHT",chatMessage);
                messageTextbox.Text = "";
            }
        }
    }
}
