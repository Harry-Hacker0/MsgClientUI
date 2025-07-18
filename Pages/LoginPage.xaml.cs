using System;
using System.Collections.Generic;
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

namespace MsgClientUI.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        MainWindow window;

        public LoginPage()
        {
            InitializeComponent();
            window = (MainWindow)Application.Current.MainWindow;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            window.Connect();
        }
    }
}
