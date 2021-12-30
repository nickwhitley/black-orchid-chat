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
        private List<IUser> _users;
        public List<IUser> Users { get => _users;  set => _users = LoadListFromCSV(); }
        
        public string FilePath => @"./Logs/UserLog.csv";

        private Dictionary<IUser, string> _tempConnections;
        public Dictionary<IUser, string> TempConnections { get => _tempConnections; }

        public UserLogger()
        {

        }

        public bool AuthenticateUser(IUser user)
        {
            
            if (!Users.Contains(user))
            {
                return true;
            } else if(Users.Any(u => (u.Username == user.Username) && (u.Password == user.Password)))
            {
                return true;
            }
            return false;
        }

        public List<IUser> LoadListFromCSV()
        {
            List<IUser> output = new();
            IUser user;
            var lines = File.ReadAllLines(FilePath).ToList();

            //remove header
            lines.RemoveAt(0);

            foreach(string line in lines)
            {
                string[] values = line.Split(',');
                user = Factory.CreateUser(values[0], values[1]);            

                output.Add(user);
            }

            return output;
        }

        public void LoadUsers()
        {
            Users = LoadListFromCSV();
        }

        public void SaveListToCSV(List<IUser> users)
        {
            List<string> lines = new();
            //add file header
            lines.Add("Username,Password");
            
            foreach(var user in Users)
            {
                lines.Add($"{ user.Username },{ user.Password }");
            }

            File.WriteAllLines(FilePath, lines);
        }

        public void SaveUser(IUser user)
        {
            Users.Add(user);
        }

        public void AddConnection(IUser user, string connectionId)
        {
            _tempConnections.Add(user, connectionId);
        }

        public void RemoveConnection(IUser user)
        {
            _tempConnections.Remove(user);
        }

        public int NumOfConnections()
        {
            return _tempConnections.Count;
        }

        public string GetConnectionId(IUser user)
        {
            string value = " Connection ID not found. ";
            if (_tempConnections.TryGetValue(user, out value))
            {
                return value;
            }
            
                return value;
            
        }
    }
}
