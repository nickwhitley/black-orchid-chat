using Caliburn.Micro;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Threading.Tasks;

namespace WPFClient.Models
{
    public class ServerConnection : PropertyChangedBase, IServerConnection
    {
        private HubConnection _hubConnection;

        public HubConnection HubConnection
        {
            get => _hubConnection;
            set 
            { 
                _hubConnection = value;
                NotifyOfPropertyChange(() => HubConnection);
            }
        }

        public ServerConnection()
        {
            InitializeClientConnection();
        }
        public void InitializeClientConnection()
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(@"https://blackorchidchat.azurewebsites.net/chat")
                .Build();

            new Thread(() => connection.StartAsync().Wait());

            HubConnection = connection;
        }
    }
}
