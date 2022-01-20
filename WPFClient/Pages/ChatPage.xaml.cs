using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFClient
{
    public partial class ChatPage : Page
    {
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public BindingList<string> Users { get; private set; } = new BindingList<string>();
        private static int _numberOfUsers;

        public static int NumberOfUsers
        {
            get { return _numberOfUsers; }
            set { _numberOfUsers = value; }
        }


        public ChatPage()
        {
            InitializeComponent();
            MessagesListView.ItemsSource = Messages;
            usersListView.ItemsSource = Users;
            new Thread(ReceiveChatMessage).Start();
            new Thread(DisplayUserIsTyping).Start();
            new Thread(StopDisplayUserTyping).Start();
            new Thread(DisplayUsers).Start();

        }

        public void ReceiveChatMessage()
        {
            App._connection.On("ReceiveChatMessage", (string newMessage) =>
            {
                Messages.Add(newMessage);
            });
        }

        public void DisplayUserIsTyping()
        {
            App._connection.On("DisplayUserIsTyping", (string userTypingMessage) =>
            {
                Messages.Add(userTypingMessage);
            });
        }

        public void StopDisplayUserTyping()
        {
            App._connection.On("StopDisplayUserTyping", (string messageToRemove) =>
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

            App._connection.InvokeCoreAsync("DisplayUserIsTypingEvent", args: new[] { changesData });
        }

        private void MessageInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                App._connection.InvokeCoreAsync("BroadcastUserMessage", args: new[] { MessageInputTextBox.Text });
                MessageInputTextBox.Text = string.Empty;
            }
        }

        private void MessageInputTextBox_Initialized(object sender, EventArgs e)
        {
            MessageInputTextBox.Focus();
        }

        public void DisplayUsers()
        {
            App._connection.On("UpdateUsersList", (List<string> userNames) =>
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

                NumberOfUsers = Users.Count;
            });
        }
    }
}
