using Caliburn.Micro;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IServerConnection _connection;
        private readonly IEventAggregator _eventAggregator;
        private string statusLabel = "Server is down";
        private string userName;

        public string UserName
        {
            get => userName;
            set { userName = value; }
        }
        
        public string StatusLabel
        {
            get => statusLabel;
            set
            {
                statusLabel = value;
                NotifyOfPropertyChange(() => StatusLabel);
            }
        }

        public LoginViewModel(
            IServerConnection connection,
            IEventAggregator eventAggregator)
        {
            _connection = connection;
            _eventAggregator = eventAggregator;

            new Thread(ChangeConnectionStatus).Start();
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

        public void EnteredUserName(TextBox userNameTextBox, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                _connection.HubConnection.InvokeCoreAsync("ReceiveUsername",
                                        args: new[] { userName });

                _eventAggregator.PublishOnUIThreadAsync(new ValidUserNameEntered());
            }
        }

        // Not working
        public void ChangeConnectionStatus()
        {
            _connection.HubConnection.On("ChangeConnectionStatus", (string status) =>
            {
                if (status != null)
                {
                    StatusLabel = "Connected";
                }
                else
                    StatusLabel = "Server is down";
            });
        }
    }
}
