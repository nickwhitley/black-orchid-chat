using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Hubs
{

    public class ChatHub : Hub
    {
        /// <summary>
        /// Dictionary clientUsernames contains connectionId keys linked to username values
        /// </summary>
        public static Dictionary<string, string> clientUsernames = new Dictionary<string, string>();

        public async Task RetrieveUsername(string username)
        {
            clientUsernames.Add(Context.ConnectionId, username);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{ username } has connected.");
            await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.\n" +
                $"There are {clientUsernames.Count} user(s) online.");
        }

        public async Task SendChatMessage(string message)
        {
            string username = clientUsernames[Context.ConnectionId];
            Console.WriteLine($"message received[{username}]");

            string messageToSend = $"{username}: {message}";

            if (clientUsernames.Count < 2)
            {
                await Clients.All.SendAsync("ReceiveChatMessage", messageToSend);
                await Clients.All.SendAsync("ReceiveChatMessage", "Server: There's no one on here you big 'ol dummy.");
            }
            else
            {
                await Clients.All.SendAsync("ReceiveChatMessage", messageToSend);
            }
        }

        public async override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected.");

        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (clientUsernames.ContainsKey(Context.ConnectionId))
            {
                await Clients.All.SendAsync("ReceiveChatMessage", $"{clientUsernames[Context.ConnectionId]} has disconnected");
            }
            Console.WriteLine("Unkown Client disconnect.");
        }
    }
}
