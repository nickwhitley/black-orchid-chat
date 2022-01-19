﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace WPFClient
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
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
