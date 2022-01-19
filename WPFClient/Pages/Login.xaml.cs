using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        //public string StatusLabelText { get; set; }
        public Login()
        {
            InitializeComponent();
            //new Thread(DisplayConnectionStatus).Start();

        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            App._connection.InvokeCoreAsync("ReceiveUsername",
                                            args: new[] { userNameTextBox.Text });

            ChatPage chatPage = new ChatPage();
            NavigationService.Navigate(chatPage);
        }

        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                App._connection.InvokeCoreAsync("ReceiveUsername",
                                            args: new[] { userNameTextBox.Text });

                ChatPage chatPage = new ChatPage();
                NavigationService.Navigate(chatPage);
            }
        }

        private void userNameTextBox_Initialized(object sender, EventArgs e)
        {
            userNameTextBox.Focus();
        }
        //public void DisplayConnectionStatus()
        //{
        //    StatusLabelText = App._connection.State.ToString();
        //    statusLabel.Content = StatusLabelText;
        //}

    }
}
