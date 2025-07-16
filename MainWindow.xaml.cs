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
using MsgClientUI.Interface;
using MsgClientUI.Infrastructure;

namespace MsgClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User user;
        MessageList newList;
        Dictionary <int,Page> pages = new Dictionary <int,Page> ();

        public MainWindow()
        {
            InitializeComponent();
            newList = new();
            messageList.ItemsSource = newList;
            Connect();

            if (messageList.Items.Count > 0) messageList.ScrollIntoView(messageList.Items[messageList.Items.Count - 1]);
            
        }

        public void Connect()
        {

            user = new User();
            user.UpdateMessages += AddMessage;
        }

        // event handler
        public void AddMessage(object sender, MessageItem IsSuccessful)
        {
            Debug.WriteLine($"To Be Added: {IsSuccessful.Content}");

            newList.Add(IsSuccessful);
            if (messageList.Items.Count > 0) messageList.ScrollIntoView(messageList.Items[messageList.Items.Count - 1]);
        }

        public void OnSelect(int chatValue)
        {
            if (pages.TryGetValue(chatValue, out Page chat))
            {

            }
            else
            {

            }
        }


        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            user.Connect();
        }

        private void Create_Channel_Click(object sender, RoutedEventArgs e)
        {
            messageList.Visibility = Visibility.Collapsed;

            ActionMessage actionMessage = new ActionMessage();

            //user.SendMessage("ACT", actionMessage);
        }

        private void Message_Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.author = user.GetCode();
                chatMessage.timestamp = DateTime.Now;
                chatMessage.content = messageTextbox.Text;
                chatMessage.channel = "Channel 4";

                user.SendMessage("CHT", chatMessage);
                messageTextbox.Text = "";
            }
        }
    }
}