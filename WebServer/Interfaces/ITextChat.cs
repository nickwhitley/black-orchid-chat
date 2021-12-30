using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    public interface ITextChat
    {
        Task ReceiveUserLoginInfo(string username, string password);
        Task BroadcastUserCount(ITempConnections connections);
        Task BroadcastUserConnected(string username);
        Task BroadcastUserMessage(string message);
    }
}
