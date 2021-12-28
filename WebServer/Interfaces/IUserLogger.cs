using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebServer.User;

namespace WebServer.Interfaces
{
    interface IUserLog
    {
        List<IUser> Users { get; set; }
        string FilePath { get; }

        void SaveUser(IUser client)
        {
            //save to a file
            
        }

        void LoadUsers()
        {
            //load from a file
        }
    }
}
