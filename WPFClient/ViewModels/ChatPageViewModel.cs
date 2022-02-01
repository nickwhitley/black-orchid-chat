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
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public BindingList<string> Users { get; private set; } = new BindingList<string>();
        public object MessageInputTextBox { get; private set; }

        private readonly IServerConnection _connection;
        private readonly IEventAggregator _eventAggregator;

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
        private void MessageInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dictionary<string, object> changesData = new Dictionary<string, object>();
            changesData.Add("Offset", e.Changes.First().Offset);
            changesData.Add("AddedLength", e.Changes.First().AddedLength);
            changesData.Add("RemovedLength", e.Changes.First().RemovedLength);

            _connection.HubConnection.InvokeCoreAsync("DisplayUserIsTypingEvent", args: new[] { changesData });
        }
        public void MessageInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _connection.HubConnection.InvokeCoreAsync("BroadcastUserMessage", args: new[] { MessageInputTextBox });
                MessageInputTextBox = string.Empty;
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
    
