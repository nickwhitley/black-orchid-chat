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

        public async Task ReceiveUserLoginInfo(string username)
        {
            Console.WriteLine(username);
            IUser user = Factory.CreateUser(username, Context.ConnectionId);
            
                _userLogger.AddUser(user);
                
                await BroadcastUserConnected(user.Username);
                await BroadcastUserCount(_userLogger.NumberOfUsers());
            
        }

        public async Task BroadcastUserConnected(string username)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{username} has connected.");
            await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.");
        }

        public async Task BroadcastUserCount(int numberOfUsers)
        {
            if (numberOfUsers == 2)
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {numberOfUsers - 1} other users online.");
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

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected.");
            return Task.CompletedTask;
        }


        public async override Task OnDisconnectedAsync(Exception exception)
        {
            try{
                IUser user = _userLogger.TryGetUser(Context.ConnectionId);
                await Clients.All.SendAsync("ReceiveChatMessage", $"{ user.Username } has disconnected");
            } catch (Exception ex)
            {
                Console.WriteLine("Unknown client disconnected.");
            }
            
        }
    }
}
