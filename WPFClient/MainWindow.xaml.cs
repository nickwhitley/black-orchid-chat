using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Windows;

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        private static string _status;

        public static string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public MainWindow()
        {
            new Thread(GetServerStatus).Start();
            InitializeComponent();
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
