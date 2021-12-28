using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebServer.Interfaces
{
    public interface IUserAuthenticator
    {
        bool AuthenticateUserPassword(IUser user, string password);
        bool DoesUserExist(IUser user);
    }
}
