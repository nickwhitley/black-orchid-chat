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
            return new User(username, connectionId);           
        }
    }
}
