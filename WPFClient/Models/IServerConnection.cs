using Microsoft.AspNetCore.SignalR.Client;

namespace WPFClient.Models
{
    public interface IServerConnection
    {
        HubConnection HubConnection { get; set; }

        void InitializeClientConnection();
    }
}