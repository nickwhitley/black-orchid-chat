using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace WebServer.Interfaces
{
    public interface IUserLogger
    {

        void AddUser(IUser user);

        void RemoveUser(IUser user);

        List<IUser> GetAllUsers();

        IUser TryGetUser(string connectionId);

        int NumberOfUsers();
    }
}
