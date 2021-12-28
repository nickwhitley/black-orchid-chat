using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer.User
{
    public class User : IUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string connectionId { get; set; }
    }
}
