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
        private List<IUser> Users = new List<IUser>();
        
        public UserLogger()
        {
            
        }

        public void AddUser(IUser user)
        {
            Users.Add(user);
            
        }

        public void RemoveUser(IUser user)
        {
            Users.Remove(user);
            
        }

        public List<IUser> GetAllUsers()
        {
            return Users;
        }

        public IUser TryGetUser(string connectionId)
        {
            var user = Users.First(c => c.ConnectionId == connectionId);
            if(user == null)
            {
                throw new Exception(connectionId + " not found in list of Users.");
            }
            return user;

        }

        public List<string> GetAllUsernames()
        {
            var usernames = new List<string>();
            foreach(var user in Users)
            {
                usernames.Add(user.Username);
            }
            return usernames;
        }

        public int NumberOfUsers()
        {
            return Users.Count;
        }
    }
}
