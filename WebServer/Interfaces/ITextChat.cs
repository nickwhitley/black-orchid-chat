using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    public interface ITextChat
    {
        Task ReceiveUserLoginInfo(string username);
        Task BroadcastUserCount(int numberOfUsers);
        Task BroadcastUserConnected(string username);
        Task BroadcastUserMessage(string message);
    }
}
