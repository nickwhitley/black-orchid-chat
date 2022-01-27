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
        public static string Status { get; set; } = string.Empty;
        public Login()
        {
            statusLabel.Content = Status;
            InitializeComponent();

            new Thread(RecieveConnectionStatus).Start();
        }

        private void RecieveConnectionStatus()
        {
            App._connection.On("ReceiveConnectionStatus", (string status) =>
            {
                if (App._connection.ConnectionId == status)
                {
                    Status = "Connected";
                }
                else
                {
                    Status = "Disconnected";
                };
            });
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
