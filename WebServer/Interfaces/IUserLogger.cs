using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace WebServer.Interfaces
{
    public interface IUserLogger : IUserAuthenticator, IFileProcessor, ITempConnections
    {
        List<IUser> Users { get; set; }

        void SaveUser(IUser client);

        void LoadUsers();
    }
}
