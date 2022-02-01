using Caliburn.Micro;
using Microsoft.AspNetCore.SignalR.Client;
using WPFClient.Models;
using WPFClient.Views;

namespace WPFClient.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IServerConnection _connection;
        private readonly IEventAggregator _eventAggregator;
        private string statusLabel = string.Empty;
        private string userName;

        public string UserName
        {
            get => userName;
            set { userName = value; }
        }
        
        public string StatusLabel
        {
            get => statusLabel;
            set => Set(ref statusLabel, value);
        }

        public LoginViewModel(
            IServerConnection connection,
            IEventAggregator eventAggregator)
        {
            _connection = connection;
            _eventAggregator = eventAggregator;

            StatusLabel = connection.HubConnection.State.ToString();
        }
        public bool CanLogin(string userName)
        {
            return !string.IsNullOrWhiteSpace(userName);
        }

        public void Login(string userName)
        {
            _connection.HubConnection.InvokeCoreAsync("ReceiveUsername",
                                        args: new[] { userName });

            _eventAggregator.PublishOnUIThreadAsync(new ValidUserNameEntered());
        }

        //private void loginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (userNameTextBox.Text != "")
        //    {
        //        connection.HubConnection.InvokeCoreAsync("ReceiveUsername",
        //                                args: new[] { userNameTextBox.Text });

        //        ChatPageView chatPage = new ChatPageView();
        //        NavigationService.Navigate(chatPage);
        //    }
        //    else
        //    {
        //        usernameLabel.Content = "Cannot be blank, idiot";
        //    }
        //}

        //private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        if (userNameTextBox.Text != "")
        //        {
        //            App._connection.InvokeCoreAsync("ReceiveUsername",
        //                                    args: new[] { userNameTextBox.Text });

        //            ChatPageView chatPage = new ChatPageView();
        //            NavigationService.Navigate(chatPage);
        //        }
        //        else
        //        {
        //            usernameLabel.Content = "Cannot be blank, idiot";
        //        }
        //    }
        //}

        //private void userNameTextBox_Initialized(object sender, EventArgs e)
        //{
        //    userNameTextBox.Focus();
        //}

        //private void userNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    usernameLabel.Content = string.Empty;
        //}
    }
}
