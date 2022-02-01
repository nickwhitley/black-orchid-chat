using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace WPFClient.Models
{
    public interface IServerConnection
    {
        HubConnection HubConnection { get; set; }

        void InitializeClientConnection();
    }
}