using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class ChatPage : Page, INotifyPropertyChanged
    {
        private string message = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Message
        {
            get { return message; }
        }
        public ChatPage()
        {
            InitializeComponent();
        }

        private void SubmitMessageButton_Click(object sender, RoutedEventArgs e)
        {
            App._connection.InvokeCoreAsync("BroadcastUserMessage", args: new[] { MessageInputTextBox.Text });

            App._connection.On("ReceiveChatMessage", (string newMessage) =>
            {
                message += newMessage + Environment.NewLine;
                PropertyChanged("Message");
            });
        }

        public static void ReceiveChatMessage(string message)
        {

        }
    }
}
