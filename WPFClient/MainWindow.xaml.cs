using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        public static string Username { get; set; }
        private static HubConnectionState _status;

        public static HubConnectionState Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public MainWindow()
        {
            Status = App._connection.State;
            InitializeComponent();
        }
    }
}
