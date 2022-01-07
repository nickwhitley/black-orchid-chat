using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Factories;
using WebServer.Interfaces;


namespace WebServer.ClientHandler
{
    public class UserLogger : IUserLogger
    {
        private List<IUser> _users = new List<IUser>();
        public List<IUser> Users { get => _users; }
        
        public UserLogger()
        {
            
        }

        public void AddUser(IUser user)
        {
            _users.Add(user);
        }

        public void RemoveUser(IUser user)
        {
            _users.Remove(user);
        }

        public IUser TryGetUser(string connectionId)
        {
            foreach (var user in Users)
            {
                if(user.ConnectionId == connectionId)
                {
                    return user;
                }
            }

            throw new Exception(connectionId + " not found in list of users.");
        }

        public int NumberOfUsers()
        {
            return _users.Count;
        }
    }
}
