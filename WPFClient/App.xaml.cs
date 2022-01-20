using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace WPFClient
{
    public partial class App : Application
    {
        public static HubConnection _connection;
        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(@"https://webserver20220120092346.azurewebsites.net/chat")
                .Build();

            connection.StartAsync();

            _connection = connection;

            base.OnStartup(e);
        }
    }
}
