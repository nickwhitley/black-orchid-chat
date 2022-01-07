using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace WebServer.Interfaces
{
    public interface IUserLogger
    {
        List<IUser> Users { get; }

        void AddUser(IUser user);

        void RemoveUser(IUser user);

        IUser TryGetUser(string connectionId);

        int NumberOfUsers();
    }
}
