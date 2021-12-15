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
        /// Dictionary contains connectionId keys linked to username values. Persistant across server instance.
        /// </summary>
        public static Dictionary<string, string> connectionDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary holds username keys and password values to validate user login.
        /// </summary>
        private static Dictionary<string, string> passwordDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Retreive client's sent username and distributes it to required methods.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task RetrieveUsername(string username)
        {
            SaveUsername(username);
            await BroadcastUserConnected(username);
            await UserCountNotification();  
        }

        public void RetrieveUserPassword(string username, string password)
        {
            SaveUsernameAndPassword(username, password);
        }

        /// <summary>
        /// Saves clients username to connections dictionary.
        /// </summary>
        /// <param name="username"></param>
        private void SaveUsername(string username)
        {
            connectionDictionary.Add(Context.ConnectionId, username);
            
        }

        /// <summary>
        /// Saves clients username and password to password dictionary.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool SaveUsernameAndPassword(string username, string password)
        {

            //IN PROGRESS - NEED TO ADD EXISTING USER CHECKING!
            return passwordDictionary.TryAdd(username, password);
        }

        /// <summary>
        /// Notifies all current users that a new user has connected.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task BroadcastUserConnected(string username)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{username} has connected.");
            await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.");
        }

        /// <summary>
        /// Displays how many other users are currently connected when a user enters chat.
        /// </summary>
        /// <returns></returns>
        private async Task UserCountNotification()
        {
            if (clientUsernames.Count == 2)
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {(clientUsernames.Count) - 1} other users online.");
            }
        }

        /// <summary>
        /// Sends message to clients to display in chat window.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendChatMessage(string message)
        {
            string username = connectionDictionary[Context.ConnectionId];
            string messageToSend = $"{username}: {message}";

            Console.WriteLine($"message received[{username}]");

            await Clients.All.SendAsync("ReceiveChatMessage", messageToSend);
        }

        /// <summary>
        /// Notifies server when a user has connected and is in login screen.
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Broadcasts to clients when a user has disconnected. Notify server when a unknown user has disconnected.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (connectionDictionary.ContainsKey(Context.ConnectionId))
            {
                await Clients.All.SendAsync("ReceiveChatMessage", $"{connectionDictionary[Context.ConnectionId]} has disconnected");
            }
            Console.WriteLine("Unkown Client disconnect.");
        }
    }
}
