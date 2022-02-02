using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WPFClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFClient.ViewModels
{
    public class ChatPageViewModel : Screen
    {
        public BindingList<string> Users { get; private set; } = new BindingList<string>();
        private ObservableCollection<string> _messages;
        private string _message;
        private readonly IServerConnection _connection;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set 
            { 
                _messages = value; 
                NotifyOfPropertyChange(() => Messages);
            }
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
        }
        public void ReceiveChatMessage()
        {
            _connection.HubConnection.On("ReceiveChatMessage", (string newMessage) =>
            {
                Messages.Add(newMessage);
            });

        }

        public void DisplayUserIsTyping()
        {
            _connection.HubConnection.On("DisplayUserIsTyping", (string userTypingMessage) =>
            {
                Messages.Add(userTypingMessage);
            });
        }

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
    }
}


//private void MessageInputTextBox_Initialized(object sender, EventArgs e)
//{
//    MessageInputTextBox.Focus();
//}

//public void DisplayUsers()
//{
    //App._connection.On("UpdateUsersList", (List<string> userNames) =>
    //{
    //    foreach (var localUser in Users)
    //    {
    //        if (!userNames.Contains(localUser))
    //        {
    //            Users.Remove(localUser);
    //        }
    //    }
    //    foreach (var remoteUser in userNames)
    //    {
    //        if (!Users.Contains(remoteUser))
    //        {
    //            Users.Add(remoteUser);
    //        }
    //    }

    //    NumberOfUsers = Users.Count;
    //});
//}
    
