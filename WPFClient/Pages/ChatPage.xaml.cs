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
            Messages.Add("TestMessage");
            InitializeComponent();
            //DataContext = this;
            MessagesListBox.ItemsSource = Messages;
            new Thread(ReceiveChatMessage).Start();
            
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
    }
}
