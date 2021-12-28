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
        public static IUser CreateUser()
        {
            return new User();
        }

        public static IUserLogger CreateUserLogger()
        {
            return new UserLogger();
        }
    }
}
