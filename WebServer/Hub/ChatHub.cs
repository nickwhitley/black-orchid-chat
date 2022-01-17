using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async Task ReceiveUsername(string username)
        {
            Console.WriteLine($"User connected: {username} {Context.ConnectionId}");
            IUser user = Factory.CreateUser(username, Context.ConnectionId);
            
            _userLogger.AddUser(user);
                
            await BroadcastUserConnected(user.Username);
            await BroadcastUserCount();
            await UpdateClientUsersOnlineList();

        }

        public async Task BroadcastUserConnected(string username)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{username} has connected.");
            await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.");
        }

        public async Task BroadcastUserCount()
        {
            int numberOfUsers = _userLogger.NumberOfUsers();
            if (numberOfUsers == 2)
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {numberOfUsers - 1} other Users online.");
            }

        }

        public async Task BroadcastUserMessage(string message)
        {
            IUser user;
            try
            {
                user = _userLogger.TryGetUser(Context.ConnectionId);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Clients.Caller.SendAsync("ReceiveChatMessage", "An error occured, please try again");
                return;
            }
            
            string username = user.Username;
            string messageToSend = $"{username}: {message}";

            Console.WriteLine($"message received[{username}]");

            await Clients.All.SendAsync("ReceiveChatMessage", messageToSend);
        }

        public async Task UpdateClientUsersOnlineList()
        {
            var userList = _userLogger.GetAllUsers();
            await Clients.All.SendAsync("UpdateUsersList", userList);
        }

        public async Task DisplayUserIsTypingEvent(Dictionary<string, object> changesData)
        {
            //changesData contains: Offest, AddedLength, RemovedLength as keys.
            changesData.TryGetValue("Offset", out var offset);
            changesData.TryGetValue("AddedLength", out var addedLength);
            changesData.TryGetValue("RemovedLength", out var removedLength);

            int offsetConverted = int.Parse(offset.ToString());
            int addedLengthConverted = int.Parse(addedLength.ToString());
            int removedLengthConverted = int.Parse(removedLength.ToString());

            var caller = _userLogger.TryGetUser(Context.ConnectionId).Username;
            string message = $"{ caller } is typing...";

            if (offsetConverted == 0 && addedLengthConverted == 1)
            {
                
                await Clients.AllExcept(Context.ConnectionId).SendAsync("DisplayUserIsTyping", message);
            }

            if(offsetConverted == 0 && removedLengthConverted == 1) 
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("StopDisplayUserTyping", message);
            }
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected.");
            return Task.CompletedTask;
        }


        public async override Task OnDisconnectedAsync(Exception exception)
        {
            try{
                IUser user = _userLogger.TryGetUser(Context.ConnectionId);
                _userLogger.RemoveUser(user);
                Console.WriteLine($"{user.Username} has disconnected.");
                await Clients.All.SendAsync("ReceiveChatMessage", $"{ user.Username } has disconnected");
                await UpdateClientUsersOnlineList();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Unknown client disconnected.");
            }
            
        }
    }
}
