using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WPFClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using WPFClient.Views;

namespace WPFClient.ViewModels
{
    public class ChatPageViewModel : Screen
    {
        private ObservableCollection<string> _users = new ObservableCollection<string>();
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        private readonly IServerConnection _connection;
        private readonly IEventAggregator _eventAggregator;
        private string _message;

        public ObservableCollection<string> Users
        {
            get => _users;
            set { _users = value; }
        }

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set { _messages = value; }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public ChatPageViewModel(IServerConnection connection,
                                 IEventAggregator eventAggregator)
        {
            _connection = connection;
            _eventAggregator = eventAggregator;

            new Thread(ReceiveChatMessage).Start();
            new Thread(UpdateUsers).Start();
            new Thread(DisplayUserIsTyping).Start();
            new Thread(StopDisplayUserTyping).Start();
        }
        public void ReceiveChatMessage()
        {
            _connection.HubConnection.On("ReceiveChatMessage", (string newMessage) =>
            {
                Messages.Add(newMessage);
            });

        }

        // Not working
        public void DisplayUserIsTyping()
        {
            _connection.HubConnection.On("DisplayUserIsTyping", (string userTypingMessage) =>
            {
                Messages.Add(userTypingMessage);
            });
        }

        // Not working
        public void StopDisplayUserTyping()
        {
            _connection.HubConnection.On("StopDisplayUserTyping", (string messageToRemove) =>
            {
                Messages.Remove(messageToRemove);
            });
        }
        public void MessageBoxTextChanged(TextChangedEventArgs e)
        {
            Dictionary<string, object> changesData = new Dictionary<string, object>();
            changesData.Add("Offset", e.Changes.First().Offset);
            changesData.Add("AddedLength", e.Changes.First().AddedLength);
            changesData.Add("RemovedLength", e.Changes.First().RemovedLength);

            _connection.HubConnection.InvokeCoreAsync("DisplayUserIsTypingEvent", args: new[] { changesData });
        }
        public void MessageEntered(KeyEventArgs args, TextBox source)
        {
            if (args.Key == Key.Enter)
            {
                _connection.HubConnection.InvokeCoreAsync("BroadcastUserMessage", args: new[] { source.Text });
                Message = string.Empty;
            }
        }
        public void UpdateUsers()
        {
            _connection.HubConnection.On("UpdateUsersList", (List<string> userNames) =>
            {
                foreach (var localUser in Users)
                {
                    if (!userNames.Contains(localUser))
                    {
                        Users.Remove(localUser);
                    }
                }
                foreach (var remoteUser in userNames)
                {
                    if (!Users.Contains(remoteUser))
                    {
                        Users.Add(remoteUser);
                    }
                }
            });
        }

        // Not working
        public void MessageBoxInitialized(ChatPageView view)
        {
            view.Message.Focus();
        }
    }
}


