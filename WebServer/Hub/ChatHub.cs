using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.ClientHandler;
using WebServer.Factories;
using WebServer.Interfaces;

namespace WebServer.Hubs
{

    public class ChatHub : Hub, ITextChat
    {

        IUserLogger _userLogger;

        public ChatHub(IUserLogger userLogger)
        {
            _userLogger = userLogger; 
        }

        public async Task ReceiveUserLoginInfo(string username, string password)
        {
            IUser user = Factory.CreateUser(username, password);
            if (_userLogger.AuthenticateUser(user))
            {
                _userLogger.SaveUser(user);
                _userLogger.AddConnection(Context.ConnectionId, user);
                await BroadcastUserConnected(user.Username);
                await BroadcastUserCount(_userLogger);
            }
        }

        public async Task BroadcastUserConnected(string username)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{username} has connected.");
            await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.");
        }

        public async Task BroadcastUserCount(ITempConnections connections)
        {
            if (connections.NumOfConnections() == 2)
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {(connections.NumOfConnections()) - 1} other users online.");
            }

        }

        public async Task BroadcastUserMessage(string message)
        {
            _userLogger.TempConnections.TryGetValue(Context.ConnectionId, out IUser user);
            string username = user.Username;
            string messageToSend = $"{username}: {message}";

            Console.WriteLine($"message received[{username}]");

            await Clients.All.SendAsync("ReceiveChatMessage", messageToSend);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected.");
            return Task.CompletedTask;
        }


        public async override Task OnDisconnectedAsync(Exception exception)
        {
            
            if (_userLogger.TempConnections.TryGetValue(Context.ConnectionId, out IUser user))
            {
                await Clients.All.SendAsync("ReceiveChatMessage", $"{ user.Username } has disconnected");
            }
            Console.WriteLine("Unkown Client disconnect.");
        }



        //public static Dictionary<string, string> connectionDictionary = new Dictionary<string, string>();


        //private static Dictionary<string, string> passwordDictionary = new Dictionary<string, string>();


        //public async Task ReceiveUserLoginInfo(string username, string password)
        //{
        //    IUser user = Factory.CreateUser(username, password);
        //    if (!_userLogger.AuthenticateUser(user))
        //    {

        //    } else

        //        _userLogger.SaveUser(Factory.CreateUser(username, password));
        //        await BroadcastUserConnected(username);
        //        await UserCountNotification();

        //}

        //public async Task RetreiveUsername(string username)
        //{
        //    await BroadcastUserConnected(username);
        //    await UserCountNotification(); 
        //}


        //private void SaveUser(string username, string password)
        //{
        //    connectionDictionary.Add(Context.ConnectionId, username);
        //    passwordDictionary.Add(username, password);
        //}



        //private bool AuthenticateUser(string username, string password)
        //{
        //    if (passwordDictionary.ContainsKey(username) && !passwordDictionary[username].Equals(password))
        //    {
        //        return false;
        //    }
        //    else if (!passwordDictionary.ContainsKey(username))
        //    {
        //        if (username.Length > 18 || password.Length > 14)
        //        {

        //            return false;
        //        } else
        //        {
        //            SaveUser(username, password);
        //            return true;
        //        }
        //    }
        //    return true;
        //}


        //private async Task BroadcastUserConnected(string username)
        //{
        //    await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{username} has connected.");
        //    await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.");
        //}


        //private async Task UserCountNotification()
        //{
        //    if (connectionDictionary.Count == 2)
        //    {
        //        await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
        //    }
        //    else
        //    {
        //        await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {(connectionDictionary.Count) - 1} other users online.");
        //    }
        //}


        //public async Task SendChatMessage(string message)
        //{
        //    string username = connectionDictionary[Context.ConnectionId];
        //    string messageToSend = $"{username}: {message}";

        //    Console.WriteLine($"message received[{username}]");

        //    await Clients.All.SendAsync("ReceiveChatMessage", messageToSend);
        //}



    }
}
