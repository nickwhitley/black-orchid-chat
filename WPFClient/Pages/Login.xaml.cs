using Microsoft.AspNetCore.SignalR.Client;
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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            App._connection.InvokeCoreAsync("ReceiveUserLoginInfo",
                                            args: new[] { userNameTextBox.Text });
            //App._connection.On("ReceiveChatMessage", (string message) =>
            //{
            //    WriteLine($"{ message }");
            //});

            ChatPage chatPage = new ChatPage();
            NavigationService.Navigate(chatPage);
        }
    }
}
