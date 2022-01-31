using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace WPFClient
{
    public partial class Login : Page
    {
        private string _connectionStatus = App._connection.State.ToString();
        public string ConnectionStatus {
            get { return _connectionStatus; } 
            set { ConnectionStatus = value; } 
        }
        public Login()
        {
            InitializeComponent();
            new Thread(GetConnectionStatus).Start();
            statusLabel.Content = ConnectionStatus;
        }

        private void GetConnectionStatus()
        {
            //ConnectionStatus = App._connection.State.ToString();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (userNameTextBox.Text != "")
            {
                App._connection.InvokeCoreAsync("ReceiveUsername",
                                        args: new[] { userNameTextBox.Text });

                ChatPage chatPage = new ChatPage();
                NavigationService.Navigate(chatPage);
            }
            else
            {
                usernameLabel.Content = "Cannot be blank, idiot";
            }
        }

        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (userNameTextBox.Text != "")
                {
                    App._connection.InvokeCoreAsync("ReceiveUsername",
                                            args: new[] { userNameTextBox.Text });

                    ChatPage chatPage = new ChatPage();
                    NavigationService.Navigate(chatPage);
                }
                else
                {
                    usernameLabel.Content = "Cannot be blank, idiot";
                }
            }
        }

        private void userNameTextBox_Initialized(object sender, EventArgs e)
        {
            userNameTextBox.Focus();
        }

        private void userNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            usernameLabel.Content = string.Empty;
        }
    }
}
