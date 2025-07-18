using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MsgClientUI.Messages;
using MsgClientUI.Infrastructure;
using MsgClientUI.Pages;
using MsgClientUI.Bindings;

namespace MsgClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConnectionHandler user;
        Dictionary <string, UserChat> pages = new Dictionary <string, UserChat> ();
        HomePage homepage;

        public MainWindow()
        {
            InitializeComponent();
        }

        public ConnectionHandler Connection
        {
            get { return user; }
            set { user = value; }
        }

        public Dictionary<string, UserChat> Pages
        {
            get { return pages; }
            set { pages = value; }
        }

        public void Connect()
        {

            user = new ConnectionHandler();
            user.UpdateMessages += AddMessage;
            user.UpdateUsers += AddUser;
            if (user.IsConnected)
            {
                homepage = new HomePage();
                mainApplicationFrame.Content = homepage;
            }
        }

        // Add Message Event Handler
        public void AddMessage(object sender, MessageItem IsSuccessful)
        {
            Debug.WriteLine($"To Be Added: {IsSuccessful.Content}");

            if (pages.TryGetValue(IsSuccessful.Channel, out UserChat value)) 
            {
                Debug.WriteLine($"MainWindow UserChat: {IsSuccessful.Channel}");
                value.AddChatMessage(IsSuccessful);
            }
            else
            {
                Debug.WriteLine($"MainWindow New UserChat: {IsSuccessful.Channel}");
                UserChat newChat = new UserChat();
                newChat.SetPageID(IsSuccessful.Channel);
                pages.Add(IsSuccessful.Channel, newChat);
                newChat.AddChatMessage(IsSuccessful);
            }
        }

        // Add User Event Handler
        public void AddUser(object sender, UserItem IsSuccessful)
        {
            Debug.WriteLine($"To Be Added: {IsSuccessful.AuthorID}");

            homepage.AddUser(IsSuccessful);
            Debug.WriteLine($"Added: {IsSuccessful.AuthorID}");
        }
    }
}