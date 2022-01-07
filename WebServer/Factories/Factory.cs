using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.ClientHandler;
using WebServer.Interfaces;

namespace WebServer.Factories
{
    public static class Factory
    {
        public static IUser CreateUser(string username, string connectionId)
        {
            User user = new User();
            user.Username = username;
            user.ConnectionId = connectionId;
            return user;
        }
    }
}
