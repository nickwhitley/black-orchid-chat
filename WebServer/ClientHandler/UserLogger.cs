using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Factories;
using WebServer.Interfaces;


namespace WebServer.ClientHandler
{
    public class UserLogger : IUserLogger, IFileProcessor, IUserAuthenticator
    {
        private List<IUser> _users;
        public List<IUser> Users { get => _users;  set => _users = LoadListFromCSV(); }
        
        public string FilePath => @"./Logs/UserLog.csv";

        public UserLogger()
        {

        }

        
        public bool AuthenticateUserPassword(IUser user, string password)
        {
            throw new NotImplementedException();
        }

        public bool DoesUserExist(IUser user)
        {
            throw new NotImplementedException();
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
                user = Factory.CreateUser();
                user.Username = values[0];
                user.Password = values[1];
                user.ConnectionId = values[2];

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
            lines.Add("Username,Password,ConnectionID");
            
            foreach(var user in Users)
            {
                lines.Add($"{ user.Username },{ user.Password },{ user.ConnectionId }");
            }

            File.WriteAllLines(FilePath, lines);
        }

        public void SaveUser(IUser user)
        {
            Users.Add(user);
        }
    }
}
