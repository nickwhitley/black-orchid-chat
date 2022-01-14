using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer.ClientHandler
{
    public class User : IUser
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }

        public User(string username, string connectionId)
        {
            Username = username;
            ConnectionId = connectionId;
        }
        

        
    }
}
