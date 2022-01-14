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
        private List<IUser> users = new List<IUser>();
        
        public UserLogger()
        {
            
        }

        public void AddUser(IUser user)
        {
            users.Add(user);
            
        }

        public void RemoveUser(IUser user)
        {
            users.Remove(user);
            
        }

        public IUser TryGetUser(string connectionId)
        {
            //foreach (var user in users)
            //{
            //    if(user.ConnectionId == connectionId)
            //    {
            //        return user;
            //    }
            //}

            var user = users.First(c => c.ConnectionId == connectionId);
            if(user == null)
            {
                throw new Exception(connectionId + " not found in list of users.");
            }
            return user;

        }

        public int NumberOfUsers()
        {
            return users.Count;
        }
    }
}
