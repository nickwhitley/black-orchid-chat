using Caliburn.Micro;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Threading.Tasks;

namespace WPFClient.Models
{
    public class ServerConnection : PropertyChangedBase, IServerConnection
    {
        private static HubConnection _hubConnection;

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

            connection.StartAsync();

            HubConnection = connection;
        }
    }
}
