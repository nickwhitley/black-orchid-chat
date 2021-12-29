﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.ClientHandler;
using WebServer.Interfaces;

namespace WebServer.Hubs
{

    public class ChatHub : Hub
    {

        IUserLogger _userLogger;

        public ChatHub(IUserLogger userLogger)
        {
            _userLogger = userLogger;
            
        }

        /// <summary>
        /// Dictionary contains connectionId keys linked to username values. Persistant across server instance.
        /// </summary>
        public static Dictionary<string, string> connectionDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary holds username keys and password values to validate user login.
        /// </summary>
        private static Dictionary<string, string> passwordDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Receives user information upon login and distributes it to required methods.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task ReceiveUserLoginInfo(string username, string password)
        {
            if (AuthenticateUser(username, password))
            {
                await BroadcastUserConnected(username);
                await UserCountNotification();
            }
        }

        public async Task RetreiveUsername(string username)
        {
            await BroadcastUserConnected(username);
            await UserCountNotification(); 
        }

        /// <summary>
        /// Saves user's information in dictionaries.
        /// </summary>
        /// <param name="username"></param>
        private void SaveUser(string username, string password)
        {
            connectionDictionary.Add(Context.ConnectionId, username);
            passwordDictionary.Add(username, password);
        }


        /// <summary>
        /// Checks if user exists in user dictionaries and verifies password entered.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> true if user either doesn't exist in dictionary and has enter correctly formatted information
        /// or if user does exist and has enter their correct password.</returns>
        private bool AuthenticateUser(string username, string password)
        {
            if (passwordDictionary.ContainsKey(username) && !passwordDictionary[username].Equals(password))
            {
                return false;
            }
            else if (!passwordDictionary.ContainsKey(username))
            {
                if (username.Length > 18 || password.Length > 14)
                {

                    return false;
                } else
                {
                    SaveUser(username, password);
                    return true;
                }
            }
            return true;
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
            if (connectionDictionary.Count == 2)
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {(connectionDictionary.Count) - 1} other users online.");
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
