using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Interfaces;
using WebServer.User;

namespace WebServer.Logs
{
    public class UserLog : IUserLog
    {
        public List<IUser> Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string FilePath => throw new NotImplementedException();

        public void Save
    }
}
