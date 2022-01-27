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

            await BroadcastConnectionStatus(Context.ConnectionId);
            //await BroadcastUserCount();
            await UpdateClientUsersOnlineList();

        }

        public async Task BroadcastUserConnected(string username)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveChatMessage", $"{username} has connected.");
            await Clients.Caller.SendAsync("ReceiveChatMessage", "You have connected.");
        }

        //Added by DC and needs to be fixed
        public async Task BroadcastConnectionStatus(string callerContext)
        {
            await Clients.Caller.SendAsync("ReceiveConnectionStatus", $"{callerContext}");
        }

        //public async Task BroadcastUserCount()
        //{
        //    int numberOfUsers = _userLogger.NumberOfUsers();
        //    if (numberOfUsers == 2)
        //    {
        //        await Clients.Caller.SendAsync("ReceiveChatMessage", "There is 1 other user online.");
        //    }
        //    else
        //    {
        //        await Clients.Caller.SendAsync("ReceiveChatMessage", $"There are {numberOfUsers - 1} other Users online.");
        //    }

        //}

        public async Task BroadcastUserMessage(string message)
        {
            IUser user;
            try
            {
                user = _userLogger.TryGetUser(Context.ConnectionId);
            }
            catch (Exception ex)
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
            var userList = _userLogger.GetAllUsernames();
            await Clients.All.SendAsync("UpdateUsersList", userList);
        }

        public async Task DisplayUserIsTypingEvent(Dictionary<string, object> changesData)
        {
            //offset is the number of characters that the cursor sits from the first input position
            int offset = GetTypingEventData(changesData)[0];
            //added/removed length is the number of characater that have been added/removed when the change event occurs, usually '1'.
            int addedLength = GetTypingEventData(changesData)[1];
            int removedLength = GetTypingEventData(changesData)[2];

            var callerUsername = _userLogger.TryGetUser(Context.ConnectionId).Username;
            string message = $"{ callerUsername } is typing...";

            if (offset == 0 && addedLength >= 1)
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("DisplayUserIsTyping", message);
            }

            if (offset == 0 && removedLength >= 1)
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("StopDisplayUserTyping", message);
            }
        }

        private int[] GetTypingEventData(Dictionary<string, object> data)
        {
            data.TryGetValue("Offset", out var offset);
            data.TryGetValue("AddedLength", out var addedLength);
            data.TryGetValue("RemovedLength", out var removedLength);

            int offsetConverted = int.Parse(offset.ToString());
            int addedLengthConverted = int.Parse(addedLength.ToString());
            int removedLengthConverted = int.Parse(removedLength.ToString());

            int[] returnData = new[] { offsetConverted, addedLengthConverted, removedLengthConverted };
            return returnData;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected.");
            return Task.CompletedTask;
        }


        public async override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                IUser user = _userLogger.TryGetUser(Context.ConnectionId);
                _userLogger.RemoveUser(user);
                Console.WriteLine($"{user.Username} has disconnected.");
                await Clients.All.SendAsync("ReceiveChatMessage", $"{ user.Username } has disconnected");
                await UpdateClientUsersOnlineList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Unknown client disconnected.");
            }

        }
    }
}
