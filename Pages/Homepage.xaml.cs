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
    public partial class HomePage : Page
    {
        MainWindow window;
        UsersList usrList;

        public HomePage()
        {
            window = (MainWindow)Application.Current.MainWindow;
            usrList = new UsersList();
            InitializeComponent();
            userList.ItemsSource = usrList;
        }

        public void AddUser(UserItem item)
        {
            usrList.Add(item);

            Debug.WriteLine($"User Added Count: {usrList.Count}");
        }

        private void Create_Channel_Click(object sender, RoutedEventArgs e)
        {
            //messageList.Visibility = Visibility.Collapsed;

            ActionMessage actionMessage = new ActionMessage();

            //user.SendMessage("ACT", actionMessage);
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            window.Connection.Connect();
        }

        private void User_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            UserItem selectedItem = (UserItem)userList.SelectedItem;
            Debug.WriteLine($"HomePage UserChat: {selectedItem.AuthorID}");
            if (window.Pages.TryGetValue(selectedItem.AuthorID, out UserChat value))
            {

                Debug.WriteLine($"{selectedItem.AuthorID}");
                homepageFrame.Content = value;
                Debug.WriteLine($"Set HomePage: {value}");
            }
            else
            {

                Debug.WriteLine($"HomePage New UserChat: {selectedItem.AuthorID}");
                UserChat chat = new UserChat();
                window.Pages.Add(selectedItem.AuthorID, chat);
                chat.SetPageID(selectedItem.AuthorID);
                homepageFrame.Content = chat;
                Debug.WriteLine($"Created and set.");
            }
        }
    }
}
