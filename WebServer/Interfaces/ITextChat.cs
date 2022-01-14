﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    public interface ITextChat
    {
        Task ReceiveUsername(string username);
        Task BroadcastUserCount();
        Task BroadcastUserConnected(string username);
        Task BroadcastUserMessage(string message);
    }
}
