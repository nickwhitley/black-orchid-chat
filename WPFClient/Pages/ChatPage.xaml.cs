using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Threading;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class ChatPage : Page
    {

        BindingList<string> messages = new BindingList<string>();

        ObservableCollection<string> messagesObservable = new ObservableCollection<string>();
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        
        

        public ChatPage()
        {
            InitializeComponent();
            //DataContext = this;
            MessagesListBox.ItemsSource = Messages;
            new Thread(ReceiveChatMessage).Start();
            new Thread(DisplayUserIsTyping).Start();
            new Thread(StopDisplayUserTyping).Start();
            
        }

        private void SubmitMessageButton_Click(object sender, RoutedEventArgs e)
        {
            
            App._connection.InvokeCoreAsync("BroadcastUserMessage", args: new[] { MessageInputTextBox.Text });
            
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
    }
}
