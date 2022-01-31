using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Windows;

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        private static string _status;
        // TODO Donald: Are you wanting this to be server status or connection status?
        public static string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            new Thread(GetServerStatus).Start();
        }

        //This doesn't work yet
        public void GetServerStatus()
        {
            if (App._connection.State == HubConnectionState.Disconnected)
            {
                Status = "Server is currently down";
            }
        }
    }
}
